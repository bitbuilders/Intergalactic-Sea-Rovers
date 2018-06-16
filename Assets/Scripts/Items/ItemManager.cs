using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] List<GameObject> m_itemTemplates = null;

    public ItemEntity GetItem(string name)
    {
        ItemEntity item = null;

        foreach (GameObject i in m_itemTemplates)
        {
            ItemEntity it = i.GetComponent<ItemEntity>();
            if (it != null && it.Name == name)
            {
                Instantiate(i);
                item = it;
            }
        }

        return item;
    }
}
