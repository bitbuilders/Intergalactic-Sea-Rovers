using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionManager : MonoBehaviour
{
    [SerializeField] GameObject m_options = null;

    public void ToggleOptions()
    {
        if (m_options.activeSelf)
            m_options.SetActive(false);
        else
            m_options.SetActive(true);
    }
}