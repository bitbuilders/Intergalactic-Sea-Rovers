using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Interactable3DObject
{
    [SerializeField] [Range(0.0f, 10.0f)] float m_detonationTime = 1.0f;
    [SerializeField] [Range(1.0f, 100.0f)] float m_damage = 10.0f;
    [SerializeField] Collider m_explosionTrigger = null;
    [SerializeField] Collider m_collisionCollider = null;
    [SerializeField] Color m_startColor;
    [SerializeField] Color m_endColor;
    [SerializeField] AnimationCurve m_colorCurve = null;
    [SerializeField] AnimationCurve m_scaleCurve = null;

    public float Damage { get { return m_damage; } }

    Vector3 m_launchOffset;
    ParticleSystem m_particleSystem;
    Material m_material;
    GameObject m_mesh;

    new private void Start()
    {
        base.Start();
        m_launchOffset = Vector3.up * 2.0f;
        m_particleSystem = GetComponentInChildren<ParticleSystem>();
        m_material = GetComponentInChildren<Renderer>().material;
        m_mesh = GetComponentInChildren<MeshRenderer>().gameObject;
        m_explosionTrigger.enabled = false;
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_rigidbody.isKinematic = false;
            m_rigidbody.useGravity = true;
            Entity e = other.GetComponentInParent<Entity>();
            Vector3 dir = transform.position - e.transform.position;
            dir += m_launchOffset;
            m_rigidbody.AddForce(dir.normalized * m_launchForce, ForceMode.Impulse);
            Quaternion rotation = e.transform.rotation * Quaternion.Euler(new Vector3(90.0f, 90.0f, 0.0f));
            StartCoroutine(RotateX(rotation, 2.0f));
            StartCoroutine(Explode());
        }
    }

    IEnumerator RotateX(Quaternion targetRot, float speed)
    {
        for (float i = 0.0f; i < 1.0f; i+= Time.deltaTime * speed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, i);
            yield return null;
        }

        transform.rotation = targetRot;
    }

    IEnumerator Explode()
    {
        GetComponent<InteractionBlink>().Reset();
        GetComponent<InteractionBlink>().enabled = false;
        for (float i = 0.0f; i <= 1.0f; i+= Time.deltaTime / m_detonationTime)
        {
            float a = m_colorCurve.Evaluate(i);
            Color c = Color.Lerp(m_startColor, m_endColor, a);
            m_material.color = c;

            float s = m_scaleCurve.Evaluate(i);
            m_mesh.transform.localScale = Vector3.one * s;
            yield return null;
        }
        //yield return new WaitForSeconds(m_detonationTime);
        StartCoroutine(HandleTrigger());
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.isKinematic = true;
        m_collisionCollider.enabled = false;
        m_particleSystem.Play(true);
        GetComponentInChildren<MeshRenderer>().enabled = false;
        AudioManager.Instance.PlayClip("BarrelExplosion", transform.position, false, transform);
        m_material.color = Color.white;
        while (m_particleSystem.IsAlive())
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator HandleTrigger()
    {
        m_explosionTrigger.enabled = true;
        yield return new WaitForSeconds(0.25f);
        m_explosionTrigger.enabled = false;
    }
}
