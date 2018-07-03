using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGame : MonoBehaviour
{
    [SerializeField] GameObject m_opponent = null;
    [SerializeField] HealthBar m_p1HBar = null;
    [SerializeField] HealthBar m_p2HBar = null;

    void Start()
    {
        BattleManager.Instance.LoadBattle(m_opponent.transform);
    }

    private void Update()
    {
        Player p = FindObjectOfType<Player>();
        Enemy e = m_opponent.GetComponent<Enemy>();
        m_p1HBar.UpdateHealth(p.Health / p.MaxHealth);
        m_p2HBar.UpdateHealth(e.Health / e.MaxHealth);
    }

    private void LateUpdate()
    {
        Player p = FindObjectOfType<Player>();
        Enemy e = m_opponent.GetComponent<Enemy>();
        if (!p.Alive && !e.Alive)
        {

        }
        else if (!p.Alive)
        {

        }
        else if (!e.Alive)
        {

        }
    }
}
