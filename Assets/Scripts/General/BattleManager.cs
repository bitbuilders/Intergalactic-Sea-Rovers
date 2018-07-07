using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    Player m_player = null;
    public Player Player { get; set; }
    public Enemy Enemy { get; set; }
    public Entity Winner { get; set; }

    static BattleManager ms_instance = null;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (ms_instance == null)
        {
            ms_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        m_player = FindObjectOfType<Player>();
    }

    public void LoadBattle()
    {
        m_player.transform.position = Vector3.zero;
        m_player.SwapControllers("3D");
        bool splitscreen = (Game.Instance.GameMode == Game.Mode.MULTIPLAYER);
        SplitscreenManager.Instance.SetSplitscreen(splitscreen, m_player);
    }
}
