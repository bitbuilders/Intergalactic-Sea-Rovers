using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    [SerializeField] Entity m_parent = null;
    [SerializeField] string[] m_hitableTags = null;


    private void OnTriggerEnter(Collider other)
    {
        if (IsValidTag(other.tag))
        {
            Enemy e = other.GetComponent<Enemy>();
            Player p = other.GetComponent<Player>();
            float damage = 0.0f;
            if (e != null)
            {
                damage = e.Inventory.EquippedItems.Weapon.Damage;
            }
            else
            {
                damage = p.Inventory.EquippedItems.Weapon.Damage;
            }
            m_parent.TakeDamage(damage);
        }
    }

    bool IsValidTag(string tag)
    {
        bool valid = false;

        foreach (string s in m_hitableTags)
        {
            if (s == tag)
            {
                valid = true;
                break;
            }
        }

        return valid;
    }
}
