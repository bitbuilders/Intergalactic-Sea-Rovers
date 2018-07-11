using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 60.0f)] float m_forceRate = 30.0f;
    [SerializeField] [Range(0.0f, 500.0f)] float m_forceMagnitude = 100.0f;
    [SerializeField] Rigidbody m_lanternBody = null;
    [SerializeField] GameObject m_lanternAnchor = null;
    [SerializeField] GameObject m_lanternMesh = null;
    [SerializeField] bool m_pushOnce = false;

    float m_forceTime = 0.0f;

    private void Start()
    {
        m_forceTime = m_forceRate;
        if (m_pushOnce)
            Shake();
    }

    private void Update()
    {
        if (!m_pushOnce)
        {
            m_forceTime += Time.deltaTime;

            if (m_forceTime >= m_forceRate)
            {
                m_forceTime = 0.0f;
                Shake();
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 dir = m_lanternAnchor.transform.position - m_lanternBody.transform.position;
        m_lanternMesh.transform.up = dir;
    }

    public void Shake()
    {
        Vector2 d = Random.insideUnitCircle;
        Vector3 dir = new Vector3(d.x, 0.0f, d.y);
        Vector3 force = dir.normalized * m_forceMagnitude;
        m_lanternBody.AddForce(force, ForceMode.Impulse);
    }
}
