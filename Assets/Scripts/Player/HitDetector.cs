﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    [SerializeField] string[] m_hitableTags = null;

    Entity m_parent = null;

    private void Start()
    {
        m_parent = GetComponent<Entity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger");
        if (IsValidTag(other.tag))
        {
            Entity e = other.GetComponentInParent<Entity>();
            e.WeaponCollider.SetColliderInactive(e.Inventory.EquippedItems.Weapon.Name);

            float damage = 0.0f;
            if (e != null)
            {
                damage = e.Inventory.EquippedItems.Weapon.Damage;
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