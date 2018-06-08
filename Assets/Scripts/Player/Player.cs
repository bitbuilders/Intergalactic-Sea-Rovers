using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] Controller m_controller = null;

    public Controller Controller { get { return m_controller; } }

    void Start()
    {
        CanMove = true;
    }

    private void Update()
    {
        if (CanMove)
        {
            m_controller.Move();
        }
    }
}
