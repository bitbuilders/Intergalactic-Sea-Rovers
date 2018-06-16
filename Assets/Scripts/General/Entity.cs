using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] public Interactee m_interacteeInfo = null;
    public Inventory Inventory { get; protected set; }
    public bool CanMove { get; set; }

    protected void Initialize(Entity entity)
    {
        Inventory = GetComponent<Inventory>();
        Inventory.Owner = entity;
    }
}
