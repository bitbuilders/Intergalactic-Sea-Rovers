using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitscreenManager : Singleton<SplitscreenManager>
{
    public enum SplitscreenType
    {
        VERTICAL_CUT,
        HORIZONTAL_CUT
    }

    [SerializeField] SplitscreenType m_screenType = SplitscreenType.VERTICAL_CUT;
    [SerializeField] Camera m_p1Cam = null;
    [SerializeField] Camera m_p2Cam = null;
    [SerializeField] Enemy m_enemy = null;

    public void SetSplitscreen(bool splitscreen, Player player)
    {
        m_p1Cam.GetComponent<CameraController>().Entity(player);
        m_p1Cam.GetComponent<CameraController>().Target(m_enemy.transform);
        player.Camera = m_p1Cam.GetComponent<CameraController>();
        if (splitscreen)
        {
            if (m_screenType == SplitscreenType.VERTICAL_CUT)
            {
                m_p1Cam.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
                m_p2Cam.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
            }
            else
            {
                m_p1Cam.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
                m_p2Cam.rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
            }

            m_p2Cam.GetComponent<CameraController>().Entity(m_enemy);
            m_p2Cam.GetComponent<CameraController>().Target(player.transform);
            m_enemy.Camera = m_p2Cam.GetComponent<CameraController>();
            m_p2Cam.gameObject.SetActive(true);
            m_p2Cam.enabled = true;
            m_enemy.IsHuman = true;
        }
        else
        {
            m_p1Cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            m_p2Cam.enabled = false;
            m_enemy.IsHuman = false;
        }
    }
}
