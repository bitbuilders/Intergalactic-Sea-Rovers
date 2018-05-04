using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Controller m_controller = null;

    public Controller Controller { get { return m_controller; } }

    void Start()
    {
        InteractionManager.Instance.Interact(null);
    }

    private void Update()
    {
        m_controller.Move();
    }
}
