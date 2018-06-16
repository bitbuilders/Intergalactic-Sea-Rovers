using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEntity : MonoBehaviour
{
    public enum Rarity
    {
        BASIC,
        ADVANCED,
        LEGENDARY,
        GODLIKE
    }

    [Header("Shop Info")]
    [SerializeField] Sprite m_image = null;
    [SerializeField] string m_name = "Generic Item";
    [SerializeField] [Range(0, 9999)] int m_goldValue = 10;
    [SerializeField] Rarity m_rarity = Rarity.BASIC;

    public Sprite Image { get { return m_image; } }
    public string Name { get { return m_name; } }
    public int GoldValue { get { return m_goldValue; } }
    public Rarity Rareness { get { return m_rarity; } }
}
