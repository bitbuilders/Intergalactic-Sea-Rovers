using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Interactable3DObject
{
    [SerializeField] [Range(0.0f, 10.0f)] float m_detonationTime = 1.0f;

    Vector3 m_launchOffset;

    new private void Start()
    {
        base.Start();
        m_launchOffset = Vector3.up * 2.0f;
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
        yield return new WaitForSeconds(m_detonationTime);

    }
}
