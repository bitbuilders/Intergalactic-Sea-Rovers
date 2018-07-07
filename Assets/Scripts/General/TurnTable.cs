using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTable : MonoBehaviour
{
    [SerializeField] [Range(-360.0f, 360.0f)] float m_rotationSpeed = 90.0f;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(Vector3.up * m_rotationSpeed * Time.deltaTime);
    }
}
