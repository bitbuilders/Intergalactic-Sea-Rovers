using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIcon : MonoBehaviour
{
    [SerializeField] Image m_icon = null;

    ItemEntity m_item;

    public void Initialize(ItemEntity item)
    {
        m_item = item;
        m_icon.sprite = m_item.Image;
    }
}
