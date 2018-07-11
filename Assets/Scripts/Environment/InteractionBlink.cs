using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBlink : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 10.0f)] float m_blinkSpeed = 1.5f;
    [SerializeField] Color m_baseColor = Color.green;

    Renderer m_renderer;
    Material m_material;
    
    void Start()
    {
        m_renderer = GetComponentInChildren<Renderer>();
        m_material = m_renderer.material;
    }
    
    void Update()
    {
        float emission = Mathf.PingPong(Time.time * m_blinkSpeed, 1.3f) + 0.0f;
        Color finalColor = m_baseColor * Mathf.LinearToGammaSpace(emission);
        m_material.SetColor("_EmissionColor", finalColor);
    }
}
