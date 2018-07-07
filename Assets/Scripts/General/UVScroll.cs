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

    private void Start()
    {
        m_material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        x += Time.deltaTime * m_horizontalSpeed;
        y += Time.deltaTime * m_verticalSpeed;
        m_material.SetTextureOffset("_MainTex", new Vector2(x, y));
    }
}
