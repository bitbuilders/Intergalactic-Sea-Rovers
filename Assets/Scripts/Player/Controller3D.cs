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

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public override void Move()
    {
        Collider[] b = Physics.OverlapSphere(m_groundTouch.position, 0.1f, m_groundMask);
        OnGround = b.Length > 0;
        m_animator.SetBool("OnGround", OnGround);

        m_velocity = Vector3.zero;
        float horizontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpeed = Input.GetAxis("Vertical");
        m_velocity.x = horizontalSpeed * (1.0f - Mathf.Abs(verticalSpeed) * 0.25f); // Limits velocity if traveling diagonally
        m_velocity.z = verticalSpeed * (1.0f - Mathf.Abs(horizontalSpeed) * 0.25f); // "
        m_velocity *= Time.deltaTime * m_speed;
        m_velocity = CameraController.Instance.transform.rotation * m_velocity;
        m_velocity.y = 0.0f;

        m_animator.SetFloat("MoveSpeed", m_velocity.magnitude * 10.0f);
        if (Input.GetButton("Sprint"))
        {
            m_animator.SetBool("Walking", false);
            m_velocity *= m_runSpeed;
        }
        else
        {
            m_animator.SetBool("Walking", true);
        }

        transform.position += m_velocity;
        m_animator.SetFloat("yVelocity", m_rigidbody.velocity.y * 0.1f);


        if (Input.GetButtonDown("Jump") && OnGround)
        {
            m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (Input.GetButtonDown("AttackNormal"))
        {
            m_animator.SetTrigger("Attack_Normal");
        }

        if (m_velocity.magnitude != 0.0f)
        {
            Vector3 dir = (m_velocity + transform.position) - transform.position;
            Quaternion look = Quaternion.LookRotation(dir.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, look, Time.deltaTime * m_rotationSpeed);
        }
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
