using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGame : MonoBehaviour
{
    [SerializeField] GameObject m_opponent = null;

    void Start()
    {
        BattleManager.Instance.LoadBattle(m_opponent.transform);
    }
}
