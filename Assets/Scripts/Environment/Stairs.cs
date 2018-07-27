using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Entity e = collision.gameObject.GetComponent<Entity>();
        if (e != null)
        {
            SetEntityMoveAngle(e, false);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Entity e = collision.gameObject.GetComponent<Entity>();
        if (e != null)
        {
            SetEntityMoveAngle(e, true);
        }
    }
    
    private void SetEntityMoveAngle(Entity e, bool reset)
    {
        if (!reset)
        {
            e.GetComponent<Rigidbody>().isKinematic = true;
            e.MoveAngle = transform.rotation;
        }
        else
        {
            e.GetComponent<Rigidbody>().isKinematic = false;
            e.MoveAngle = Quaternion.identity;
        }
    }
}
