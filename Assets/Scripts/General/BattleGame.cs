using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGame : MonoBehaviour
{
    [SerializeField] GameObject m_opponent = null;
    [SerializeField] HealthBar m_p1HBar = null;
    [SerializeField] HealthBar m_p2HBar = null;

    Player m_player;
    Enemy m_enemy;

    void Start()
    {
        BattleManager.Instance.LoadBattle();
        m_player = FindObjectOfType<Player>();
        m_enemy = m_opponent.GetComponent<Enemy>();
        BattleManager.Instance.Player = m_player;
        BattleManager.Instance.Enemy = m_enemy;
    }

    private void Update()
    {
        m_p1HBar.UpdateHealth(m_player.Health / m_player.MaxHealth);
        m_p2HBar.UpdateHealth(m_enemy.Health / m_enemy.MaxHealth);
    }

    private void LateUpdate()
    {
        if (!m_player.Alive && !m_enemy.Alive)
        {
            SceneTransition.Instance.Transition(true, "Pedestal", 1.0f);
        }
        else if (!m_player.Alive)
        {
            BattleManager.Instance.Winner = m_enemy;
            DontDestroyOnLoad(m_enemy.gameObject);
            m_player.Respawn();
            Controller3DAI ai = m_enemy.GetComponent<Controller3DAI>();
            if (Game.Instance.GameMode == Game.Mode.SINGLEPLAYER)
                Destroy(ai);
            SceneTransition.Instance.Transition(true, "Pedestal", 1.0f);
        }
        else if (!m_enemy.Alive)
        {
            BattleManager.Instance.Winner = m_player;
            m_enemy.Respawn();
            SceneTransition.Instance.Transition(true, "Pedestal", 1.0f);
        }
    }
}
