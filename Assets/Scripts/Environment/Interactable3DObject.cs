using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable3DObject : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 25.0f)] protected float m_launchForce = 3.0f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_jumpResistance = 3.0f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_fallMultiplier = 3.0f;

    protected Rigidbody m_rigidbody;

    protected void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    protected void FixedUpdate()
    {
        if (m_rigidbody.velocity.y > 0.1f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_jumpResistance - 1.0f) * Time.deltaTime;
        }
        else if (m_rigidbody.velocity.y < -0.1f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_fallMultiplier - 1.0f) * Time.deltaTime;
        }
    }
}
