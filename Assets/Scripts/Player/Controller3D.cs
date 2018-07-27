using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller3D : Controller
{
    [SerializeField] [Range(1.0f, 5.0f)] float m_runSpeed = 2.5f;
    [SerializeField] [Range(0.0f, 1000.0f)] float m_jumpForce = 50.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_jumpResistance = 3.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_fallSpeed = 3.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_rotationSpeed = 3.0f;
    [SerializeField] Transform m_groundTouch = null;
    [SerializeField] Animator m_animator = null;
    [SerializeField] LayerMask m_groundMask = 0;
    
    Rigidbody m_rigidbody;
    Entity m_entity;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_entity = GetComponent<Entity>();
    }

    public override void Move(CameraController camera)
    {
        if (PauseManager.Instance.Paused)
            return;

        Collider[] b = Physics.OverlapSphere(m_groundTouch.position, 0.2f, m_groundMask);
        m_entity.OnGround = b.Length > 0;
        m_animator.SetBool("OnGround", m_entity.OnGround);

        if (camera == null)
            return;

        m_velocity = Vector3.zero;
        float horizontalSpeed = Input.GetAxis(m_entity.PlayerNumber + "_Horizontal");
        float verticalSpeed = Input.GetAxis(m_entity.PlayerNumber + "_Vertical");
        m_velocity.x = horizontalSpeed; // Limits velocity if traveling diagonally
        m_velocity.z = verticalSpeed; // "
        m_velocity = camera.transform.rotation * m_velocity;
        m_velocity.y = 0.0f;
        m_velocity.Normalize();
        m_velocity *= Time.deltaTime * (m_speed + m_entity.SpeedModifier);
        
        m_animator.SetFloat("MoveSpeed", m_velocity.magnitude * 10.0f);
        if (Input.GetButton(m_entity.PlayerNumber + "_Sprint"))
        {
            m_animator.SetBool("Walking", false);
            m_velocity *= m_runSpeed;
        }
        else
        {
            m_animator.SetBool("Walking", true);
        }

        if (Input.GetButtonDown(m_entity.PlayerNumber + "_Jump") && m_entity.OnGround)
        {
            m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!camera.m_locked)
        {
            if (m_velocity.magnitude != 0.0f)
            {
                Vector3 dir = (m_velocity + transform.position) - transform.position;
                Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, look, Time.deltaTime * m_rotationSpeed);
            }
        }
        else
        {
            Vector3 dir = camera.m_target.transform.position - transform.position;
            dir.y = 0.0f;
            Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, look, Time.deltaTime * m_rotationSpeed * 4.0f);
        }

        m_velocity = m_entity.MoveAngle * m_velocity;
        transform.position += m_velocity;
        //m_rigidbody.AddForce(m_velocity * 1000.0f);
        m_animator.SetFloat("yVelocity", m_rigidbody.velocity.y * 0.1f);
    }

    public override void PhysicsUpdate()
    {
        if (m_rigidbody.velocity.y > 0.01f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_jumpResistance - 1.0f) * Time.deltaTime;
        }
        else if (m_rigidbody.velocity.y < -0.01f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_fallSpeed - 1.0f) * Time.deltaTime;
        }
    }
}
