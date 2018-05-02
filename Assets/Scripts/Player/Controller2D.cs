using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : Controller
{
    public override void Move()
    {
        Vector2 velocity = Vector2.zero;
        float horizontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpeed = Input.GetAxis("Vertical");
        velocity.x = horizontalSpeed * (1.0f - verticalSpeed * 0.25f); // Limits velocity if traveling diagonally
        velocity.y = verticalSpeed * (1.0f - horizontalSpeed * 0.25f); // "
        velocity *= Time.deltaTime * m_speed;

        transform.position += (Vector3)velocity;
    }
}
