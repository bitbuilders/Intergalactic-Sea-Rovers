using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelAnimator : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 60.0f)] float m_frameRate = 30.0f;
    [SerializeField] Mesh[] m_frames = null;

    MeshFilter m_meshFilter;
    float m_time = 0.0f;
    int m_frameIndex = 0;

    private void Start()
    {
        m_meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if (m_time >= 1.0f / m_frameRate && m_frames.Length > 0)
        {
            m_time = 0.0f;
            m_frameIndex++;
            m_frameIndex = m_frameIndex % m_frames.Length;
            m_meshFilter.mesh = m_frames[m_frameIndex];
        }
    }
}
