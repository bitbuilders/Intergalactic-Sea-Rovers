using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button m_playButton = null;
    [SerializeField] Button m_gameModeSelectionB = null;
    [SerializeField] Button m_playerModeSelectionB = null;
    [SerializeField] Button m_storyModeSelectionB = null;
    [SerializeField] GameObject m_gameModeSelection = null;
    [SerializeField] GameObject m_playerModeSelection = null;
    [SerializeField] GameObject m_storyModeSelection = null;

    private void Start()
    {
        m_playButton.Select();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (m_gameModeSelection.activeSelf)
                m_gameModeSelectionB.onClick.Invoke();
            else if (m_playerModeSelection.activeSelf)
                m_playerModeSelectionB.onClick.Invoke();
            else if (m_storyModeSelection.activeSelf)
                m_storyModeSelectionB.onClick.Invoke();
        }
    }

    public void Select(Button b)
    {
        b.Select();
    }

    public void QuitGame()
    {
        Game.Instance.Quit();
    }

    public void Load(string level)
    {
        AudioManager.Instance.StopClip("MenuMusic");
        AudioManager.Instance.StopClip("BattleMusic");
        AudioManager.Instance.PlayClip("BattleMusic", Vector3.zero, true, null);
        SceneTransition.Instance.Transition(true, level);
    }

    public void ReturnToMainMenu()
    {
        AudioManager.Instance.StopClip("BattleMusic");
        AudioManager.Instance.PlayClip("MenuMusic", Vector3.zero, true, null);
        SceneTransition.Instance.Transition(true, "MainMenu");
    }

    public void PlayHoverSound()
    {
        AudioManager.Instance.PlayClip("UIHover", Vector3.zero, false, transform, 1.0f);
    }

    public void PlayClickSound()
    {
        AudioManager.Instance.PlayClip("UIClick", Vector3.zero, false, transform, 1.0f);
    }

    public void SetGameMode(string mode)
    {
        Game.Instance.GameMode = (Game.Mode)Enum.Parse(typeof(Game.Mode), mode.ToUpper());
    }
}
