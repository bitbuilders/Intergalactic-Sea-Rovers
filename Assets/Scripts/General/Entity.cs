using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public enum PlayerType
    {
        P1,
        P2
    }


    [SerializeField] public Interactee m_interacteeInfo = null;
    [SerializeField] WeaponCollider m_weaponCollider = null;
    [SerializeField] protected Animator m_animator;
    [SerializeField] PlayerType m_playerType = PlayerType.P1;

    public Inventory Inventory { get; protected set; }
    public PlayerType PlayerNumber { get { return m_playerType; } }
    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }
    public bool CanMove { get; set; }
    public bool Alive { get { return Health > 0.0f; } }
    public bool OnGround { get; set; }
    public WeaponCollider WeaponCollider { get { return m_weaponCollider; } }
    public Controller Controller { get; protected set; }
    public CameraController Camera { get; set; }


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
        AudioManager.Instance.PlayClip("SwordSlice", transform.position, false, transform);
        m_animator.SetTrigger("TakeDamage");
        if (!Alive)
        {
            Respawn();
        }
    }
}
