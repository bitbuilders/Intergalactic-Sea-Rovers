using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Pedestal : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_winningText = null;

    void Start()
    {
        Entity e = FindObjectOfType<Entity>();
        e.transform.parent = transform;
        e.transform.localPosition = Vector3.zero;
        e.GetComponent<Rigidbody>().velocity = Vector3.zero;

        string text = "";
        if (e.name == "Player")
        {
            if (Game.Instance.GameMode == Game.Mode.MULTIPLAYER)
                text = "Player 1 Wins!";
            else
                text = "You Win!";
        }
        else
        {
            if (Game.Instance.GameMode == Game.Mode.MULTIPLAYER)
                text = "Player 2 Wins!";
            else
                text = "You Lose :(";
        }

        m_winningText.text = text;
    }
}
