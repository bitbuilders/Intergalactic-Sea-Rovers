using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] public Interactee m_interacteeInfo = null;
    public Inventory Inventory { get; protected set; }
    public float Health { get; protected set; }
    public bool CanMove { get; set; }

    protected void Initialize(Entity entity)
    {
        Inventory = GetComponent<Inventory>();
        Inventory.Owner = entity;
    }

    protected void SetEquippedWeaponMesh(MeshFilter filter)
    {
        filter.mesh = Inventory.EquippedItems.Weapon.GetComponent<MeshFilter>().mesh;
        filter.GetComponent<MeshRenderer>().material = Inventory.EquippedItems.Weapon.GetComponent<MeshRenderer>().material;
        filter.GetComponent<MeshCollider>().sharedMesh = Inventory.EquippedItems.Weapon.GetComponent<MeshFilter>().mesh;
    }

    public abstract void TakeDamage(float damage);
}
