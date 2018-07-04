﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [SerializeField] Controller m_controller = null;
    [SerializeField] GameObject m_2DCharacter = null;
    [SerializeField] GameObject m_3DCharacter = null;
    [SerializeField] MeshFilter m_weaponMesh = null;

    public Controller Controller { get { return m_controller; } }

    Rigidbody m_rigidbody;

    static Player ms_instance = null;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (ms_instance == null)
            ms_instance = this;
        else
            Destroy(gameObject);

        CanMove = true;
        base.Initialize(this);
        m_rigidbody = GetComponent<Rigidbody>();

        Inventory.EquipWeapon(Inventory.Weapons[0]);
        SetEquippedWeaponMesh(m_weaponMesh);
        GetComponent<AttackController>().Initialize();
    }

    private void Update()
    {
        if (CanMove)
        {
            m_controller.Move();
        }
    }

    private void FixedUpdate()
    {
        m_controller.PhysicsUpdate();
    }

    public void SwapControllers(string controller)
    {
        if (controller.ToLower() == "2d")
        {
            m_controller = GetComponent<Controller2D>();
            m_2DCharacter.SetActive(true);
            m_3DCharacter.SetActive(false);
            m_rigidbody.useGravity = false;
        }
        else if (controller.ToLower() == "3d")
        {
            m_controller = GetComponent<Controller3D>();
            m_2DCharacter.SetActive(false);
            m_3DCharacter.SetActive(true);
            m_rigidbody.useGravity = true;
        }
    }
}
