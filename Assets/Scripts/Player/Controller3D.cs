using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller3D : Controller
{
    [SerializeField] [Range(0.0f, 1000.0f)] float m_jumpForce = 50.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_jumpResistance = 3.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_fallSpeed = 3.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_rotationSpeed = 3.0f;
    [SerializeField] Transform m_groundTouch = null;
    [SerializeField] LayerMask m_groundMask = 0;

    public bool OnGround { get; private set; }

    Rigidbody m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public override void Move()
    {
        Collider[] b = Physics.OverlapSphere(m_groundTouch.position, 0.1f, m_groundMask);
        OnGround = b.Length > 0;

        m_velocity = Vector3.zero;
        float horizontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpeed = Input.GetAxis("Vertical");
        m_velocity.x = horizontalSpeed * (1.0f - verticalSpeed * 0.25f); // Limits velocity if traveling diagonally
        m_velocity.z = verticalSpeed * (1.0f - horizontalSpeed * 0.25f); // "
        m_velocity *= Time.deltaTime * m_speed;
        m_velocity = transform.rotation * m_velocity;

        transform.position += m_velocity;

        if (Input.GetButtonDown("Jump") && OnGround)
        {
            m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }
        
        Quaternion look = Quaternion.LookRotation(m_velocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, look, Time.deltaTime * m_rotationSpeed);
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
