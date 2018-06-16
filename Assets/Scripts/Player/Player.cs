using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] Controller m_controller = null;

    public Controller Controller { get { return m_controller; } }

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
    }

    private void Update()
    {
        if (CanMove)
        {
            m_controller.Move();
        }
    }
}
