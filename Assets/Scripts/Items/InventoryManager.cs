using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] RectTransform m_inventoryUI = null;
    [SerializeField] Transform m_iconLocation = null;
    [SerializeField] GameObject m_iconTemplate = null;
    [SerializeField] Inventory m_inventory = null;

    Vector2 m_origin;
    Vector2 m_dimensions;
    float m_iconDim;

    private void Start()
    {
        m_iconDim = m_iconTemplate.GetComponent<RectTransform>().sizeDelta.x;
        m_dimensions.x = ((RectTransform)m_inventoryUI).sizeDelta.x / m_iconDim;
        m_dimensions.y = ((RectTransform)m_inventoryUI).sizeDelta.y / m_iconDim;
        m_origin.x = ((RectTransform)m_inventoryUI).sizeDelta.x / 2.0f - (m_iconDim / 2.0f) - 5.0f;
        m_origin.y = -((RectTransform)m_inventoryUI).sizeDelta.y / 2.0f + (m_iconDim / 2.0f) + 5.0f;
        UpdateInventory();
    }

    void Update()
    {

    }

    private void UpdateInventory()
    {
        Vector2 pos = Vector2.zero;
        int xCount = 0;
        foreach (ItemEntity item in m_inventory.Items)
        {
            InventoryIcon icon = CreateIcon();
            icon.Initialize(item);
            icon.transform.localPosition = pos - m_origin;
            pos.x += m_iconDim;
            xCount++;
            if (xCount >= (int)m_dimensions.x)
            {
                xCount = 0;
                pos.y -= m_iconDim;
                pos.x = 0.0f;
            }
        }
    }

    private InventoryIcon CreateIcon()
    {
        GameObject i = Instantiate(m_iconTemplate, m_iconLocation);
        InventoryIcon icon = i.GetComponent<InventoryIcon>();

        return icon;
    }
}
