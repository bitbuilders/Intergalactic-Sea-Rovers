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
        Attack(true);
    }

    public void Refresh()
    {
        SetAttackTimes();
    }

    public bool Attack(bool usePlayerInput)
    {
        if (m_attackTime >= m_attackCooldown && m_entity.OnGround)
        {
            if ((usePlayerInput && Input.GetButtonDown(m_playerNumber + "_AttackNormal")) || !usePlayerInput)
            {
                m_attackTime = 0.0f;
                m_animator.SetTrigger("Attack_Normal");
                return true;
            }
        }
        return false;
    }

    private void SetAttackTimes()
    {
        m_attackCooldown = m_inventory.EquippedItems.Weapon.Cooldown;
    }
}
