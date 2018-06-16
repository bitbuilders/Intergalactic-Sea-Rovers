using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] int m_capacity = 1;
    [SerializeField] public List<ItemEntity> Items = null;

    public int Capacity { get { return m_capacity; } set { m_capacity = value; } }

    public bool ObtainItem(Weapon weapon)
    {
        if (Full())
            return false;

        Items.Add(weapon);

        return true;
    }

    public void DropItem(Weapon weapon)
    {
        Items.Remove(weapon);
    }

    private bool Full()
    {
        return Items.Count >= Capacity;
    }
}
