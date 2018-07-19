using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] [Range(0.0f, 10.0f)] float m_jumpResistance = 3.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_fallSpeed = 3.0f;
    [SerializeField] Controller3DAI m_controllerAI;
    [SerializeField] Controller3D m_controllerPlayer;
    [SerializeField] MeshFilter m_weaponMesh = null;
    [SerializeField] Transform m_groundTouch;
    [SerializeField] LayerMask m_groundMask = 0;

    public bool IsHuman
    {
        get
        {
            return Controller == m_controllerPlayer;
        }
        set
        {
            if (value)
            {
                Controller = m_controllerPlayer;
            }
            else
            {
                Controller = m_controllerAI;
            }
        }
    }

    private void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        base.Initialize(this);

        Inventory.EquipWeapon(Inventory.Weapons[0]);
        SetEquippedWeaponMesh(m_weaponMesh);
        GetComponent<AttackController>().Initialize();
    }

    private void Update()
    {
        Collider[] b = Physics.OverlapSphere(m_groundTouch.position, 0.1f, m_groundMask);
        OnGround = b.Length > 0;
        m_animator.SetBool("OnGround", OnGround);

        if (Controller != null)
        {
            Controller.Move(Camera);
        }
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
}
