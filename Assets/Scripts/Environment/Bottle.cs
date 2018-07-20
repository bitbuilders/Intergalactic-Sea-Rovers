using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : Interactable3DObject
{
    [SerializeField] [Range(0.0f, 10.0f)] float m_interactionDistance = 2.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_healAmount = 10.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_speedIncrease = 2.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_alchoholLevel = 5.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_alchoholLinger = 5.0f;

    Entity[] m_entities;

    new private void Start()
    {
        base.Start();
        m_entities = FindObjectsOfType<Entity>();
    }

    private void LateUpdate()
    {
        Entity entity = FindClosestEntity();
        if (entity == null)
            return;

        Vector3 dir = entity.transform.position + Vector3.up - transform.position;
        if (dir.magnitude <= m_interactionDistance)
        {
            bool p1 = entity.PlayerNumber == Entity.PlayerType.P1 && Input.GetButtonDown("P1_Interact");
            bool p2 = entity.PlayerNumber == Entity.PlayerType.P2 && Input.GetButtonDown("P2_Interact");

            if (p1 || p2)
            {
                entity.Heal(m_healAmount, Entity.HealSource.BOTTLE);
                entity.SpeedModifier = m_speedIncrease;
                StopAllCoroutines();
                StartCoroutine(StopSpeed(entity));
                entity.PlayDrunkParticles();
                entity.Camera.Drunk(m_alchoholLinger, m_alchoholLevel);
            }
        }
    }

    private Entity FindClosestEntity()
    {
        Entity closest = null;

        float closestDistance = float.MaxValue;
        foreach (Entity e in m_entities)
        {
            float dist = (transform.position - e.transform.position).magnitude;
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = e;
            }
        }

        return closest;
    }

    IEnumerator StopSpeed(Entity entity)
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<MeshCollider>().enabled = false;
        yield return new WaitForSeconds(m_alchoholLinger);
        entity.StopDrunkParticles();
        entity.SpeedModifier = 0.0f;
        Destroy(gameObject);
    }
}
