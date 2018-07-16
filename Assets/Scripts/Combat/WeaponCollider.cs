using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    [System.Serializable]
    public class WeaponInfo
    {
        public Collider collider;
        public TrailRenderer trail;
        public float time;
        public string weaponSound;
        public string name;
    }

    [SerializeField] List<WeaponInfo> m_weapons = null;

    private void Start()
    {
        foreach (WeaponInfo wi in m_weapons)
        {
            wi.time = wi.trail.time;
            wi.trail.time = 0.0f;
        }
    }

    public void SetColliderActive(string name)
    {
        WeaponInfo wi = GetWeaponInfoFromName(name);
        AudioManager.Instance.PlayClip(wi.weaponSound, transform.parent.position, false, transform);
        wi.collider.enabled = true;
        wi.trail.time = wi.time;
    }

    public void SetColliderInactive(string name)
    {
        WeaponInfo wi = GetWeaponInfoFromName(name);
        wi.collider.enabled = false;
        StartCoroutine(FadeTrailTime(wi.trail, 5.0f));
    }

    IEnumerator FadeTrailTime(TrailRenderer renderer, float speed)
    {
        for (float i = renderer.time; i >= 0; i-= Time.deltaTime * speed)
        {
            renderer.time = i;
            yield return null;
        }

        renderer.time = 0.0f;
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
