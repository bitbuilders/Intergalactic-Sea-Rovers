using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBlink : MonoBehaviour
{
    [SerializeField] [Range(-10.0f, 10.0f)] float m_blinkSpeed = 1.5f;
    [SerializeField] [Range(-10.0f, 10.0f)] float m_blinkMax = 1.3f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_blinkMin = 0.0f;
    [SerializeField] Color m_baseColor = Color.green;
    [SerializeField] bool m_randomizeStartingValue = true;

    Renderer m_renderer;
    Material m_material;
    float m_offset;
    
    void Start()
    {
        m_renderer = GetComponentInChildren<Renderer>();
        m_material = m_renderer.material;

        if (m_randomizeStartingValue)
            m_offset = Random.Range(m_blinkMin, m_blinkMax);
    }
    
    void Update()
    {
        float t = Time.time * m_blinkSpeed + m_offset;
        float emission = Mathf.PingPong(t, m_blinkMax - m_blinkMin) + m_blinkMin;
        Color finalColor = m_baseColor * Mathf.LinearToGammaSpace(emission);
        m_material.SetColor("_EmissionColor", finalColor);
    }
}
