using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Entity e = other.GetComponentInParent<Entity>();
        if (e != null)
        {
            e.TakeDamage(9999.0f);
            // Sucks to be you
        }
    }
}
