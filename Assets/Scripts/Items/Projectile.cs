using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Weapon Owner { get; set; }
    public Vector3 Origin { get; set; }
    public float Falloff { get; set; }

    float m_distanceTraveled = 0.0f;

    private void Update()
    {
        m_distanceTraveled = (transform.position - Origin).magnitude;

        if (m_distanceTraveled >= Falloff)
        {
            Destroy(gameObject);
        }
    }
}
