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
            Game.Instance.LoadLevel("Pedestal");
        }
        else if (!m_player.Alive)
        {
            DontDestroyOnLoad(m_enemy.gameObject);
            Destroy(m_player.gameObject);
            Game.Instance.LoadLevel("Pedestal");
        }
        else if (!m_enemy.Alive)
        {
            Destroy(m_enemy.gameObject);
            Game.Instance.LoadLevel("Pedestal");
        }
    }
}
