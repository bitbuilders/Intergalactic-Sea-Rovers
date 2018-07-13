using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponInfo
    {
        public Collider collider;
        public TrailRenderer trail;
        public string weaponSound;
        public string name;
    }

    [SerializeField] List<WeaponInfo> m_weapons = null;

    public void SetColliderActive(string name)
    {
        WeaponInfo wi = GetWeaponInfoFromName(name);
        AudioManager.Instance.PlayClip(wi.weaponSound, transform.parent.position, false, transform);
        wi.collider.enabled = true;
        wi.trail.alignment = LineAlignment.View;
    }

    public void SetColliderInactive(string name)
    {
        WeaponInfo wi = GetWeaponInfoFromName(name);
        wi.collider.enabled = false;
        wi.trail.alignment = LineAlignment.Local;
    }

    private WeaponInfo GetWeaponInfoFromName(string name)
    {
        WeaponInfo c = default(WeaponInfo);

        foreach (WeaponInfo wi in m_weapons)
        {
            if (wi.name == name)
            {
                c = wi;
                break;
            }
        }
        
        return c;
    }
}
