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
        Game.Instance.LoadLevel(level);
    }
}
