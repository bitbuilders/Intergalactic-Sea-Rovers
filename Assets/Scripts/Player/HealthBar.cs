using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider m_slider;

    private void Start()
    {
        m_slider = GetComponent<Slider>();
    }

    public void UpdateHealth(float value)
    {
        m_slider.value = value;
    }
}
