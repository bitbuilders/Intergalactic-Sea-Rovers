using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Controller m_controller = null;

    void Start()
    {

    }

    private void Update()
    {
        m_controller.Move();
    }
}
