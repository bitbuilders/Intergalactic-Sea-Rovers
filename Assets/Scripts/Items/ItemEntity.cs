using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEntity : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] Sprite m_image = null;
    [SerializeField] string m_name = "Generic Item";
    [SerializeField] [Range(0, 9999)] int m_goldValue = 10;

    public Sprite Image { get { return m_image; } }
    public string Name { get { return m_name; } }
    public int GoldValue { get { return m_goldValue; } }
}
