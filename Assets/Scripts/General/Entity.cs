using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] public Interactee m_interacteeInfo = null;
    [SerializeField] WeaponCollider m_weaponCollider = null;

    public Inventory Inventory { get; protected set; }
    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }
    public bool CanMove { get; set; }
    public bool Alive { get { return Health > 0.0f; } }
    public WeaponCollider WeaponCollider { get { return m_weaponCollider; } }


    protected void Initialize(Entity entity)
    {
        Inventory = GetComponent<Inventory>();
        Inventory.Owner = entity;
        MaxHealth = 100.0f;
        Health = MaxHealth;
    }

    protected void SetEquippedWeaponMesh(MeshFilter filter)
    {
        filter.mesh = Inventory.EquippedItems.Weapon.GetComponent<MeshFilter>().mesh;
        filter.GetComponent<MeshRenderer>().material = Inventory.EquippedItems.Weapon.GetComponent<MeshRenderer>().material;
        filter.GetComponent<MeshCollider>().sharedMesh = Inventory.EquippedItems.Weapon.GetComponent<MeshFilter>().mesh;
    }

    virtual public void Respawn()
    {
        Health = MaxHealth;
        transform.position = Vector3.zero;
    }

    virtual public void TakeDamage(float damage)
    {
        Health -= damage;
        if (!Alive)
        {
            Respawn();
        }
    }
}
