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

    public enum DamageSource
    {
        SWORD,
        EXPLOSION,
        MISC
    }

    public enum HealSource
    {
        BOTTLE,
        MISC
    }


    [SerializeField] public Interactee m_interacteeInfo = null;
    [SerializeField] WeaponCollider m_weaponCollider = null;
    [SerializeField] protected Animator m_animator;
    [SerializeField] PlayerType m_playerType = PlayerType.P1;
    [SerializeField] ParticleSystem m_drunkParticles = null;
    [SerializeField] [Range(1.0f, 10.0f)] float m_drunkSpeed = 2.0f;

    public Inventory Inventory { get; protected set; }
    public PlayerType PlayerNumber { get { return m_playerType; } }
    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }
    public float SpeedModifier { get; set; }
    public float DrunkTime { get; set; }
    public bool CanMove { get; set; }
    public bool Alive { get { return Health > 0.0f; } }
    public bool OnGround { get; set; }
    public bool Drunk { get { return DrunkTime > 0.0f; } }
    public WeaponCollider WeaponCollider { get { return m_weaponCollider; } }
    public Controller Controller { get; protected set; }
    public CameraController Camera { get; set; }

    protected Rigidbody m_rigidbody;

    protected void Update()
    {
        if (Drunk)
        {
            DrunkTime -= Time.deltaTime;
            SpeedModifier = m_drunkSpeed;
            if (DrunkTime <= 0.0f)
            {
                StopDrunkParticles();
                SpeedModifier = 0.0f;
                DrunkTime = 0.0f;
            }
        }
    }

    protected void Initialize(Entity entity)
    {
        Inventory = GetComponent<Inventory>();
        m_rigidbody = GetComponent<Rigidbody>();
        Inventory.Owner = entity;
        MaxHealth = 100.0f;
        Health = MaxHealth;
        SpeedModifier = 0.0f;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrel"))
        {
            float damage = other.GetComponent<Barrel>().Damage;
            TakeDamage(damage, DamageSource.EXPLOSION);
            Vector3 dir = transform.position + Vector3.up * 2.0f - other.transform.position;
            Vector3 force = dir.normalized * damage;
            m_rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }

    virtual public void TakeDamage(float damage, DamageSource damageSource = DamageSource.SWORD)
    {
        Health -= damage;
        PlaySoundFromDamageSource(damageSource);
        m_animator.SetTrigger("TakeDamage");
        if (!Alive)
        {
            //Respawn();
        }
    }

    private void PlaySoundFromDamageSource(DamageSource damageSource)
    {
        switch (damageSource)
        {
            case DamageSource.SWORD:
                AudioManager.Instance.PlayClip("SwordSlice", transform.position, false, transform);
                AudioManager.Instance.PlayClip("SwordPow", transform.position, false, transform);
                break;
            case DamageSource.EXPLOSION:

                break;
            case DamageSource.MISC:
                AudioManager.Instance.PlayClip("SwordPow", transform.position, false, transform);
                break;
        }
    }

    virtual public void Heal(float amount, HealSource healSource = HealSource.BOTTLE)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0.0f, MaxHealth);
        PlaySoundFromHealSource(healSource);
    }

    private void PlaySoundFromHealSource(HealSource healSource)
    {
        switch (healSource)
        {
            case HealSource.BOTTLE:
                AudioManager.Instance.PlayClip("Drink", transform.position, false, transform);
                break;
            case HealSource.MISC:
                AudioManager.Instance.PlayClip("Drink", transform.position, false, transform);
                break;
        }
    }

    public void PlayDrunkParticles()
    {
        m_drunkParticles.Play();
    }

    public void StopDrunkParticles()
    {
        m_drunkParticles.Stop();
    }
}
