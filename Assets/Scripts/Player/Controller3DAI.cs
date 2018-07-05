using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller3DAI : Controller
{
    public enum State
    {
        STRAFE,
        CHARGE,
        RETREAT
    }

    [SerializeField] [Range(1.0f, 5.0f)] float m_runSpeed = 2.5f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_attackRange = 1.25f;
    [SerializeField] [Range(0.0f, 1000.0f)] float m_jumpForce = 50.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_jumpResistance = 3.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_fallSpeed = 3.0f;
    //[SerializeField] [Range(0.0f, 10.0f)] float m_rotationSpeed = 3.0f;
    [SerializeField] Transform m_groundTouch = null;
    [SerializeField] Animator m_animator = null;
    [SerializeField] AttackController m_attackController = null;
    [SerializeField] LayerMask m_groundMask = 0;
    
    Rigidbody m_rigidbody;
    Entity m_entity;
    Player m_player;
    State m_state;
    Vector2 m_strafeMinMax;
    Vector2 m_strafeDirMinMax;
    Vector2 m_chargeAttackMinMax;
    Vector2 m_retreatMinMax;
    float m_stateTime;
    float m_strafeAngle;
    int m_attacks;
    int m_attackLimit;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_entity = GetComponent<Entity>();
        m_player = FindObjectOfType<Player>();
        m_state = State.STRAFE;
        m_strafeMinMax = new Vector2(2.0f, 5.0f);
        m_strafeDirMinMax = new Vector2(60.0f, 80.0f);
        m_chargeAttackMinMax = new Vector2(2.0f, 4.0f);
        m_retreatMinMax = new Vector2(0.25f, 0.75f);
        m_attackLimit = Random.Range((int)m_chargeAttackMinMax.x, (int)m_chargeAttackMinMax.y);
        m_attacks = 0;
        m_strafeAngle = RandomBetweenRange(m_strafeDirMinMax, true);
        m_stateTime = RandomBetweenRange(m_strafeMinMax, false);
    }

    public override void Move(CameraController camera)
    {
        Collider[] b = Physics.OverlapSphere(m_groundTouch.position, 0.1f, m_groundMask);
        m_entity.OnGround = b.Length > 0;
        m_animator.SetBool("OnGround", m_entity.OnGround);

        m_velocity = Vector3.zero;

        m_stateTime -= Time.deltaTime;
        switch (m_state)
        {
            case State.STRAFE:
                Strafe();
                break;
            case State.CHARGE:
                Charge();
                break;
            case State.RETREAT:
                Retreat();
                break;
        }

        if (m_velocity.magnitude != 0.0f)
        {
            Vector3 dir = m_player.transform.position - transform.position;
            dir.y = Mathf.Clamp(dir.y, -3.0f, 3.0f);
            Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = look;
        }

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

        transform.position += m_velocity;
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

    private void Strafe()
    {
        Vector3 dirToPlayer = m_player.transform.position - transform.position;
        Quaternion strafeRot = Quaternion.AngleAxis(m_strafeAngle, Vector3.up);
        m_velocity = strafeRot * dirToPlayer.normalized;
        m_velocity *= m_speed * Time.deltaTime;

        if (m_stateTime <= 0.0f || dirToPlayer.magnitude <= m_attackRange)
        {
            m_state = State.CHARGE;
            m_stateTime = 10.0f;
            if (dirToPlayer.magnitude > 7.0f)
            {
                Leap(transform.forward);
            }
        }
    }

    private void Charge()
    {
        Vector3 dirToPlayer = m_player.transform.position - transform.position;
        float dist = dirToPlayer.magnitude;
        if (dist > m_attackRange)
        {
            m_velocity = dirToPlayer.normalized;
            m_velocity *= (m_speed * m_runSpeed) * Time.deltaTime;
        }
        else
        {
            if (m_attackController.Attack(false))
            {
                m_attacks++;
                if (m_attacks >= m_attackLimit)
                {
                    m_state = State.RETREAT;
                    Leap(-transform.forward);
                    m_stateTime = RandomBetweenRange(m_retreatMinMax, false);
                    m_attacks = 0;
                }
            }
        }
    } 

    private void Retreat()
    {
        Vector3 dirToPlayer = m_player.transform.position - transform.position;
        m_velocity = -dirToPlayer.normalized;
        m_velocity *= (m_speed) * Time.deltaTime;

        if (m_stateTime <= 0.0f)
        {
            m_state = State.STRAFE;
            m_stateTime = RandomBetweenRange(m_strafeMinMax, false);
            m_strafeAngle = RandomBetweenRange(m_strafeDirMinMax, true);
        }
    }

    private void Leap(Vector3 direction)
    {
        m_rigidbody.velocity = Vector3.zero;
        float a = 55.0f;
        float angle = (direction.x < 0.0f) ? -a : a;
        Quaternion jumpAngle = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector3 dir = jumpAngle * direction.normalized;
        m_rigidbody.AddForce(dir * (m_jumpForce * 1.0f), ForceMode.Impulse);
    }

    private float RandomBetweenRange(Vector2 range, bool negativeChance)
    {
        float num = Random.Range(range.x, range.y);
        if (negativeChance)
            num *= (Random.Range(0, 2) == 0) ? 1.0f : -1.0f;

        return num;
    }
}
