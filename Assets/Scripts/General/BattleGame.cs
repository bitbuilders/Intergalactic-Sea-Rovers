using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGame : MonoBehaviour
{
    void Start()
    {
        BattleManager.Instance.LoadBattle();
    }
}
