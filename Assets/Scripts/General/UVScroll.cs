using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroll : MonoBehaviour
{
    [SerializeField] [Range(-10.0f, 10.0f)] float m_horizontalSpeed = -2.0f;
    [SerializeField] [Range(-10.0f, 10.0f)] float m_verticalSpeed = -2.0f;

    Material m_material;

    float x;
    float y;
    float x_speed;
    float y_speed;

    private void Start()
    {
        m_material = GetComponent<MeshRenderer>().material;
        x_speed = m_horizontalSpeed;
        y_speed = m_verticalSpeed;
    }

    private void Update()
    {
        x_speed = Mathf.PingPong(Time.time * 0.01f, Mathf.Abs(m_horizontalSpeed)) * 2.0f - Mathf.Abs(m_horizontalSpeed);
        y_speed = Mathf.PingPong(Time.time * 0.01f, Mathf.Abs(m_verticalSpeed)) * 2.0f - Mathf.Abs(m_verticalSpeed);
        x += Time.deltaTime * x_speed;
        y += Time.deltaTime * y_speed;
        m_material.SetTextureOffset("_MainTex", new Vector2(x, y));
    }
}
