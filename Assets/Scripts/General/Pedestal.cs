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
        Entity e = BattleManager.Instance.Winner;
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
            BattleManager.Instance.Player.gameObject.SetActive(false);
            if (Game.Instance.GameMode == Game.Mode.MULTIPLAYER)
                text = "Player 2 Wins!";
            else
                text = "You Lose :(";
        }

        m_winningText.text = text;
    }

    public void SetEntitiesActive()
    {
        BattleManager.Instance.Player.gameObject.SetActive(true);
        BattleManager.Instance.Player.Respawn();
        BattleManager.Instance.Player.transform.parent = null;
        DontDestroyOnLoad(BattleManager.Instance.Player.gameObject);
    }

    public void DestroyEnemy()
    {
        if (BattleManager.Instance.Enemy != null)
            Destroy(BattleManager.Instance.Enemy.gameObject);
    }
}
