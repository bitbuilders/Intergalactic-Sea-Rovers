using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void QuitGame()
    {
        Game.Instance.Quit();
    }

    public void Load(string level)
    {
        AudioManager.Instance.StopClip("MenuMusic");
        AudioManager.Instance.PlayClip("BattleMusic", Vector3.zero, true, null);
        Game.Instance.LoadLevel(level);
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
