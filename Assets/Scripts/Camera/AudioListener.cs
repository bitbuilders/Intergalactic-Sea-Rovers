using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    [SerializeField] List<GameObject> m_players = null;

    private void Start()
    {
        m_players.Add(FindObjectOfType<Player>().gameObject);
    }

    void Update()
    {
        transform.position = GetCenter();
    }

    Vector3 GetCenter()
    {
        Vector3 center = Vector3.zero;

        for (int i = m_players.Count - 1; i >= 0; i--)
        {
            center += m_players[i].transform.position;
        }
        center /= m_players.Count;
        return center;
    }
}
