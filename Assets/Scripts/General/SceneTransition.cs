using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : Singleton<SceneTransition>
{
    [SerializeField] bool m_transitioner = true;
    [SerializeField] GameObject m_tintCanvas = null;
    [SerializeField] [Range(0.0f, 10.0f)] float m_transitionSpeed = 2.0f;

    Image m_tint;

    private void Awake()
    {
        GameObject tint = Instantiate(m_tintCanvas);
        m_tint = tint.GetComponentInChildren<Image>();
        SetTintAlpha(0.0f);
        if (!m_transitioner)
        {
            Transition(false);
        }
    }

    public void Transition(bool fadeIn, string sceneTo = "", float time = 0.0f)
    {
        StartCoroutine(Fade(fadeIn, sceneTo, time));
    }

    IEnumerator Fade(bool fadeIn, string sceneTo = "", float time = 0.0f)
    {
        float t = (time <= 0.0f) ? m_transitionSpeed : time;
        if (fadeIn)
        {
            SetTintAlpha(0.0f);
            for (float i = 0.0f; i <= 1.0f; i+= Time.deltaTime * t)
            {
                SetTintAlpha(i);
                yield return null;
            }
            SetTintAlpha(1.0f);
        }
        else
        {
            SetTintAlpha(1.0f);
            for (float i = 1.0f; i >= 0.0f ; i -= Time.deltaTime * t)
            {
                SetTintAlpha(i);
                yield return null;
            }
            SetTintAlpha(0.0f);
        }

        if (!string.IsNullOrEmpty(sceneTo))
        {
            Game.Instance.LoadLevel(sceneTo);
        }
    }

    private void SetTintAlpha(float val)
    {
        Color col = m_tint.color;
        col.a = val;
        m_tint.color = col;
    }
}
