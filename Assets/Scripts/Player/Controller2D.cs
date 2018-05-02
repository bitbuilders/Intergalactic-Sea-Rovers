using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : Controller
{
    [SerializeField] [Range(0.0f, 10.0f)] float friction = 1.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float speedRamp = 2.0f;

    float horizontalSpeed = 0.0f;
    float verticalSpeed = 0.0f;

    public override void Move()
    {
        m_velocity = Vector3.zero;
        
        float horizOpp = 0.0f;
        float vertOpp = 0.0f;
        if (horizontalSpeed > 0.05f) horizOpp = -1.0f * friction;
        else if (horizontalSpeed < -0.05f) horizOpp = 1.0f * friction;
        else horizOpp = 0.0f;
        if (verticalSpeed > 0.05f) vertOpp = -1.0f * friction;
        else if (verticalSpeed < -0.05f) vertOpp = 1.0f * friction;
        else vertOpp = 0.0f;
        
        float speedModifier = speedRamp * Time.deltaTime;
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        if (horiz == 0.0f && vert == 0.0f)
        {
            if (horizontalSpeed >= -0.05f && horizontalSpeed <= 0.05f) horizontalSpeed = 0.0f;
            if (verticalSpeed >= -0.05f && verticalSpeed <= 0.05f) verticalSpeed = 0.0f;
        }
        horizontalSpeed += horiz == 0.0f ? horizOpp * speedModifier : horiz * speedModifier;
        verticalSpeed += vert == 0.0f ? vertOpp * speedModifier : vert * speedModifier;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -1.0f, 1.0f);
        verticalSpeed = Mathf.Clamp(verticalSpeed, -1.0f, 1.0f);

        m_velocity.x = horizontalSpeed * (1.0f - verticalSpeed * 0.25f); // Limits velocity if traveling diagonally
        m_velocity.y = verticalSpeed * (1.0f - horizontalSpeed * 0.25f); // "
        m_velocity *= Time.deltaTime * m_speed;

        m_speedPercentage = Mathf.Max(Mathf.Abs(horizontalSpeed), Mathf.Abs(verticalSpeed));

        transform.position += m_velocity;
    }
}
