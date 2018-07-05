using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    [SerializeField] List<GameObject> m_cameras = null;

    int m_numOfCameras;

    private void Start()
    {
        m_numOfCameras = Game.Instance.GameMode == Game.Mode.MULTIPLAYER ? 2 : 1;
    }

    void Update()
    {
        transform.position = GetCenter();
    }

    Vector3 GetCenter()
    {
        Vector3 center = Vector3.zero;

        for (int i = 0; i < m_numOfCameras; i++)
        {
            center += m_cameras[i].transform.position;
        }
        center /= m_numOfCameras;
        return center;
    }
}
