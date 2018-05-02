using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller3D : Controller
{
    public override void Move()
    {
        m_velocity = Vector3.zero;
        float horizontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpeed = Input.GetAxis("Vertical");
        m_velocity.x = horizontalSpeed * (1.0f - verticalSpeed * 0.25f); // Limits velocity if traveling diagonally
        m_velocity.y = verticalSpeed * (1.0f - horizontalSpeed * 0.25f); // "
        m_velocity *= Time.deltaTime * m_speed;

        transform.position += m_velocity;
    }
}
