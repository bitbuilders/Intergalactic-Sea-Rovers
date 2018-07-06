using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 60.0f)] float m_forceRate = 30.0f;
    [SerializeField] [Range(0.0f, 500.0f)] float m_forceMagnitude = 100.0f;
    [SerializeField] Rigidbody m_lanternBody = null;

    float m_forceTime = 0.0f;

    private void Start()
    {
        m_forceTime = m_forceRate;
    }

    private void Update()
    {
        m_forceTime += Time.deltaTime;

        if (m_forceTime >= m_forceRate)
        {
            m_forceTime = 0.0f;
            Shake();
        }
    }

    public void Shake()
    {
        Vector2 d = Random.insideUnitCircle;
        Vector3 dir = new Vector3(d.x, 0.0f, d.y);
        Vector3 force = dir.normalized * m_forceMagnitude;
        m_lanternBody.AddForce(force, ForceMode.Impulse);
    }
}
