using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemEntity
{
    public enum ItemType
    {
        RANGED,
        MELEE,
        FISTS
    }

    [Header("General")]
    [SerializeField] GameObject m_projectileTemplate = null;
    [SerializeField] Collider m_collider = null;
    [SerializeField] ItemType m_itemType = ItemType.RANGED;
    [Header("Combat Stats")]
    [SerializeField] [Range(0.0f, 30.0f)] float m_attackCooldown = 1.0f;
    [SerializeField] [Range(0.0f, 30.0f)] float m_attackDuration = 1.0f;
    [SerializeField] [Range(0.0f, 1000.0f)] float m_damage = 1.0f;
    [SerializeField] [Range(0.0f, 1000.0f)] float m_attackRange = 1.0f;
    [SerializeField] [Range(0.0f, 5000.0f)] float m_projectileSpeed = 1.0f;
    [SerializeField] [Range(0.0f, 5000.0f)] float m_projectileFalloff = 1.0f;

    public float Cooldown { get { return m_attackCooldown; } }
    public float Damage { get { return m_damage; } }
    public float Range { get { return m_attackRange; } }
    public ItemType Type { get { return m_itemType; } }


    float m_attackTime = 100.0f;

    private void Update()
    {
        m_attackTime += Time.deltaTime;
    }

    public void Attack(Vector2 direction, Vector2 origin, float power)
    {
        // Power will be between 0 and 1
        if (m_attackTime >= m_attackCooldown)
        {
            m_attackTime = 0.0f;

            switch (m_itemType)
            {
                case ItemType.RANGED:
                    Quaternion rot = Quaternion.LookRotation(direction);
                    GameObject proj = Instantiate(m_projectileTemplate, origin, rot, null);
                    Projectile projectile = proj.GetComponent<Projectile>();
                    projectile.Owner = this;
                    projectile.Origin = projectile.transform.position;
                    projectile.Falloff = m_projectileFalloff;
                    proj.GetComponent<Rigidbody>().AddForce(proj.transform.forward.normalized * m_projectileSpeed * power, ForceMode.Impulse);
                    break;
                case ItemType.MELEE:
                    m_collider.enabled = true;
                    break;
                case ItemType.FISTS:
                    m_collider.enabled = true;
                    break;
            }
        }
    }

    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(m_attackDuration);
        m_collider.enabled = false;
    }
}
