using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] int m_capacity = 1;
    [SerializeField] GameObject m_weapons = null;
    [SerializeField] GameObject m_items = null;

    public List<Weapon> Weapons { get; private set; }
    public List<ItemEntity> Items { get; private set; }
    public Entity Owner { get; set; }
    public int Capacity { get { return m_capacity; } set { m_capacity = value; } }

    private void Start()
    {
        Weapons = new List<Weapon>();
        Items = new List<ItemEntity>();
        UpdateInventory();
    }

    public bool ObtainItem(ItemEntity item)
    {
        if (Full())
            return false;

        item.transform.parent = m_items.transform;
        Items.Add(item);

        return true;
    }

    public void DropItem(ItemEntity item)
    {
        Items.Remove(item);
    }

    public bool ObtainWeapon(Weapon weapon)
    {
        if (Full())
            return false;

        weapon.transform.parent = m_weapons.transform;
        Weapons.Add(weapon);

        return true;
    }

    public void DropWeapon(Weapon weapon)
    {
        Weapons.Remove(weapon);
    }

    private bool Full()
    {
        return Items.Count + Weapons.Count >= Capacity;
    }

    private void UpdateInventory()
    {
        UpdateWeapons();
        UpdateItems();
    }

    private void UpdateWeapons()
    {
        UpdateList(m_weapons, Weapons);
    }

    private void UpdateItems()
    {
        UpdateList(m_items, Items);
    }

    private void UpdateList<T>(GameObject parent, List<T> list)
    {
        list.Clear();
        T[] ts = m_weapons.GetComponentsInChildren<T>();
        foreach (T t in ts)
        {
            list.Add(t);
        }
    }
}
