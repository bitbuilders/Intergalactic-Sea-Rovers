using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : Singleton<Game>
{
    [SerializeField] Player m_player = null;

    static Game ms_instance = null;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (ms_instance == null)
            ms_instance = this;
        else
            Destroy(gameObject);
        
        m_player = FindObjectOfType<Player>();
        m_player.gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        m_player.gameObject.SetActive(false);
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
        m_player.gameObject.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
