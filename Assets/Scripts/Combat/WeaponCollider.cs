using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponInfo
    {
        public Collider collider;
        public string name;
    }

    [SerializeField] List<WeaponInfo> m_weapons = null;

    public void SetColliderActive(string name)
    {
        GetColliderFromName(name).enabled = true;
    }

    public void SetColliderInactive(string name)
    {
        GetColliderFromName(name).enabled = false;
    }

    private Collider GetColliderFromName(string name)
    {
        Collider c = null;

        foreach (WeaponInfo wi in m_weapons)
        {
            if (wi.name == name)
            {
                c = wi.collider;
                break;
            }
        }
        
        return c;
    }
}
