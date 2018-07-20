using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] GameObject m_pauseCanvas = null;
    [SerializeField] GameObject[] m_pauseCanvasElements = null;
    [SerializeField] Button m_resumeButton = null;

    public bool Disabled { get { return SceneManager.GetActiveScene().name == "MainMenu"; } }
    public bool Paused { get { return m_pauseCanvas.activeSelf; } }
    public bool Prompting { get; private set; }
    
    static PauseManager ms_instance = null;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (ms_instance == null)
            ms_instance = this;
        else
            Destroy(gameObject);

        Prompting = false;
        UnpauseGame();
    }

    private void Update()
    {
        if (!Prompting && Input.GetButtonDown("Pause") && SceneManager.GetActiveScene().name != "MainMenu")
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (!Paused)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    public void Select(Button button)
    {
        button.Select();
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0f;
        SetElementsActive(false);
        m_pauseCanvas.SetActive(true);
        m_resumeButton.Select();
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1.0f;
        SetElementsActive(false);
        m_pauseCanvas.SetActive(false);
    }

    private void SetElementsActive(bool active)
    {
        foreach (GameObject element in m_pauseCanvasElements)
        {
            element.SetActive(active);
        }
    }

    public void QuitGame()
    {
        Game.Instance.Quit();
    }

    public void ReturnToMainMenu()
    {
        AudioManager.Instance.StopClip("BattleMusic");
        AudioManager.Instance.PlayClip("MenuMusic", Vector3.zero, true, null);
        UnpauseGame();
        Game.Instance.ReturnToMainMenu();
    }

    public void PromptPlayer(bool prompting)
    {
        Prompting = prompting;
    }

    public void PlayHoverSound()
    {
        AudioManager.Instance.PlayClip("UIHover", Vector3.zero, false, transform, 1.0f);
    }

    public void PlayClickSound()
    {
        AudioManager.Instance.PlayClip("UIClick", Vector3.zero, false, transform, 1.0f);
    }
}
