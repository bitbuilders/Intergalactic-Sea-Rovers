using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] Animator m_animator = null;

    Entity m_entity;
    Inventory m_inventory;
    float m_attackCooldown;
    float m_attackTime;
    string m_playerNumber;
    bool m_initialized = false;

    public void Initialize()
    {
        m_inventory = GetComponent<Inventory>();
        m_entity = GetComponent<Entity>();
        SetAttackTimes();
        m_attackTime = m_attackCooldown;
        m_initialized = true;
        m_playerNumber = m_entity.PlayerNumber.ToString();
    }

    private void Update()
    {
        if (!m_initialized)
            return;

        m_attackTime += Time.deltaTime;
        if (m_attackTime >= m_attackCooldown && m_entity.OnGround && Input.GetButtonDown(m_playerNumber + "_AttackNormal"))
        {
            m_attackTime = 0.0f;
            m_animator.SetTrigger("Attack_Normal");
        }
    }

    public void Refresh()
    {
        SetAttackTimes();
    }

    private void SetAttackTimes()
    {
        m_attackCooldown = m_inventory.EquippedItems.Weapon.Cooldown;
    }
}
