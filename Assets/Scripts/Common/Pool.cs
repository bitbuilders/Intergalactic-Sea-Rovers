using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : Singleton<Pool>
{
    [SerializeField] [Range(1, 100)] int m_size = 1;
    [SerializeField] GameObject m_gameObject;

    private List<GameObject> m_gameObjects = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < m_size; ++i)
        {
            GameObject go = Instantiate(m_gameObject, gameObject.transform);
            go.SetActive(false);
            m_gameObjects.Add(go);
        }
    }

    public GameObject Get()
    {
        for (int i = 0; i < m_gameObjects.Count; ++i)
        {
            if (!m_gameObjects[i].activeInHierarchy)
            {
                return m_gameObjects[i];
            }
        }

        return null;
    }

}
