using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Interactable : Entity
{
    [SerializeField] [Range(0.0f, 10.0f)] float m_interactionDistance = 1.5f;
    [SerializeField] GameObject m_indicator = null;
    [SerializeField] KeyCode m_interactionKey = KeyCode.A;

    public bool Busy { get; set; }

    protected Player m_player = null;
    GameObject m_indicatorHUB = null;
    TextMeshProUGUI m_indicatorText = null;
    float m_xDim = 0.6f;
    float m_yDim = 0.35f;
    float m_xMult = 0.3f;
    float m_yMult = 0.2f;
    bool m_movingRight = true;
    bool m_movingUp = true;
    float m_maxFontSize;
    float m_startFontSize;
    float m_fontGrowthRate = 0.1f;
    bool m_enlarging = true;
    Vector3 m_currentIndicatorPosition = Vector3.zero;

    protected void InitializeInteraction()
    {
        FindPlayer();
        CreateIndicator();
    }

    private void Update()
    {
        if (!m_player)
            return;

        m_player.CanMove = (Busy) ? false : true;

        if (DistanceFromPlayer() < m_interactionDistance && !Busy)
        {
            m_indicatorHUB.SetActive(true);
            DisplayIndicator();
            AnimateIndicator();

            if (Input.GetKeyDown(m_interactionKey))
            {
                Interact();
            }
        }
        else
        {
            m_indicatorHUB.SetActive(false);
        }
    }

    protected abstract void Interact();

    private void FindPlayer()
    {
        m_player = FindObjectOfType<Player>();
    }

    private float DistanceFromPlayer()
    {
        Vector3 direction = m_player.transform.position - transform.position;
        return direction.magnitude;
    }

    private void CreateIndicator()
    {
        m_indicatorHUB = Instantiate(m_indicator, Vector3.up * 2.0f, Quaternion.identity, transform);
        m_indicatorHUB.transform.localPosition = new Vector3(-m_xDim, -m_yDim, 0.0f);
        m_indicatorText = m_indicatorHUB.GetComponentInChildren<TextMeshProUGUI>();
        m_startFontSize = m_indicatorText.fontSize;
        m_maxFontSize = (m_indicatorText.fontSize * 1.2f);
    }

    private void DisplayIndicator()
    {
        m_indicatorText.text = "Press " + m_interactionKey.ToString() + " to interact";
    }

    private void AnimateIndicator()
    {
        Vector3 velocity = Vector3.zero;
        velocity.x += m_xMult * Time.deltaTime * 2.0f;
        velocity.y += m_yMult * Time.deltaTime * 2.0f;
        m_currentIndicatorPosition += velocity;

        if (m_movingRight) m_xMult = m_currentIndicatorPosition.x > 0.0f ? m_xMult - Time.deltaTime * 0.25f : m_xMult + Time.deltaTime * 0.25f;
        else m_xMult = m_currentIndicatorPosition.x < 0.0f ? m_xMult + Time.deltaTime * 0.25f : m_xMult - Time.deltaTime * 0.25f;
        if (m_movingUp) m_yMult = m_currentIndicatorPosition.y > 0.0f ? m_yMult - Time.deltaTime * 0.25f : m_yMult + Time.deltaTime * 0.25f;
        else m_yMult = m_currentIndicatorPosition.y < 0.0f ? m_yMult + Time.deltaTime * 0.25f : m_yMult - Time.deltaTime * 0.25f;

        if (m_currentIndicatorPosition.x > m_xDim && m_movingRight)
        {
            m_movingRight = false;
            m_xMult = 0.0f;
            m_currentIndicatorPosition.x = m_xDim;
        }
        else if (m_currentIndicatorPosition.x < -m_xDim && !m_movingRight)
        {
            m_movingRight = true;
            m_xMult = 0.0f;
            m_currentIndicatorPosition.x = -m_xDim;
        }
        if (m_currentIndicatorPosition.y > m_yDim && m_movingUp)
        {
            m_movingUp = false;
            m_yMult = 0.0f;
            m_currentIndicatorPosition.y = m_yDim;
        }
        else if (m_currentIndicatorPosition.y < -m_yDim && !m_movingUp)
        {
            m_movingUp = true;
            m_yMult = 0.0f;
            m_currentIndicatorPosition.y = -m_yDim;
        }
        
        m_indicatorHUB.transform.localPosition = (Vector3.up * 2.0f) + m_currentIndicatorPosition;

        BlinkIndicator();
    }

    private void BlinkIndicator()
    {
        if (m_enlarging)
        {
            m_indicatorText.fontSize += Time.deltaTime * m_fontGrowthRate;

            if (m_indicatorText.fontSize >= m_maxFontSize)
            {
                m_enlarging = false;
                m_indicatorText.fontSize = m_maxFontSize;
            }
        }
        else
        {
            m_indicatorText.fontSize -= Time.deltaTime * m_fontGrowthRate;

            if (m_indicatorText.fontSize <= m_startFontSize)
            {
                m_enlarging = true;
                m_indicatorText.fontSize = m_startFontSize;
            }
        }
    }
}
