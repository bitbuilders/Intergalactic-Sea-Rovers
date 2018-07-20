using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour
{
    [SerializeField] GameObject m_previousWindow = null;
    [SerializeField] Button m_previousButton = null;
    [SerializeField] GameObject m_nextWindow = null;
    [SerializeField] Button m_nextButton = null;

    void Update()
    {
        if (m_previousWindow != null && Input.GetButtonDown("Cancel"))
        {
            m_previousWindow.SetActive(true);
            gameObject.SetActive(false);
            m_previousButton.Select();
        }
        if (m_nextWindow != null && Input.GetButtonDown("Submit"))
        {
            m_nextWindow.SetActive(true);
            gameObject.SetActive(false);
            m_nextButton.Select();
        }
    }
}
