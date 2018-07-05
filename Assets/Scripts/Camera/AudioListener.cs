using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    [SerializeField] List<GameObject> m_cameras = null;
    
    void Update()
    {
        transform.position = GetCenter();
    }

    Vector3 GetCenter()
    {
        Vector3 center = Vector3.zero;

        foreach (GameObject cam in m_cameras)
        {
            center += cam.transform.position;
        }

        center /= m_cameras.Count;
        return center;
    }
}
