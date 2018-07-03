using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] [Range(0.0f, 10.0f)] float m_jumpResistance = 3.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_fallSpeed = 3.0f;
    [SerializeField] Transform m_groundTouch;
    [SerializeField] LayerMask m_groundMask = 0;

    public bool OnGround { get; protected set; }

    EnemyAI m_AI;
    EnemyPlayer m_player;
    Animator m_animator;
    Rigidbody m_rigidbody;

    public bool IsHuman
    {
        get
        {
            return m_player.enabled;
        }
        set
        {
            m_player.enabled = value;
            m_AI.enabled = !value;
        }
    }

    private void Start()
    {
        m_AI = GetComponent<EnemyAI>();
        m_player = GetComponent<EnemyPlayer>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Collider[] b = Physics.OverlapSphere(m_groundTouch.position, 0.1f, m_groundMask);
        OnGround = b.Length > 0;
        m_animator.SetBool("OnGround", OnGround);
    }

    private void FixedUpdate()
    {
        if (m_rigidbody.velocity.y > 0.01f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_jumpResistance - 1.0f) * Time.deltaTime;
        }
        else if (m_rigidbody.velocity.y < -0.01f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_fallSpeed - 1.0f) * Time.deltaTime;
        }

        m_animator.SetFloat("yVelocity", m_rigidbody.velocity.y);
    }

    public override void TakeDamage(float damage)
    {
        Health -= damage;
    }
}
