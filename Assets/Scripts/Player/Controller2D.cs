using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : Controller
{
    [SerializeField] [Range(0.0f, 10.0f)] float friction = 1.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float speedRamp = 2.0f;
    [SerializeField] LayerMask m_barrierMask = 0;

    float maxSpeed;
    float horizontalSpeed = 0.0f;
    float verticalSpeed = 0.0f;

    private void Start()
    {
        maxSpeed = speedRamp;
    }

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

        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        if (horiz * horizontalSpeed < 0.0f || vert * verticalSpeed < 0.0f) speedRamp = maxSpeed * 2.0f;
        else speedRamp = maxSpeed;

        float speedModifier = speedRamp * Time.deltaTime;
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

        m_speedPercentageHoriz = horizontalSpeed;
        m_speedPercentageVert = verticalSpeed;

        if (m_velocity.magnitude > 0.0f)
        {
            m_velocity = LimitVelocity(m_velocity);
        }
        transform.position += m_velocity;
    }
    

    private Vector3 LimitVelocity(Vector2 velocity)
    {
        Vector3 newVelocity = velocity;

        RaycastHit2D raycastHitRight = Physics2D.Raycast(transform.position, Vector3.right, 10.0f, m_barrierMask);
        RaycastHit2D raycastHitUp = Physics2D.Raycast(transform.position, Vector3.up, 10.0f, m_barrierMask);
        RaycastHit2D raycastHitLeft = Physics2D.Raycast(transform.position, Vector3.left, 10.0f, m_barrierMask);
        RaycastHit2D raycastHitDown = Physics2D.Raycast(transform.position, Vector3.down, 10.0f, m_barrierMask);

        //print("Right: " + raycastHitRight.distance + " | " + "Left: " + raycastHitLeft.distance + " | " +
        //    "Up: " + raycastHitUp.distance + " | " + "Down: " + raycastHitDown.distance + " | ");
        if (raycastHitRight.distance < 0.3f && raycastHitRight.distance > 0.0f && velocity.x > 0.0f)
        {
            newVelocity.x = 0.0f;
            horizontalSpeed = 0.0f;
        }
        if (raycastHitUp.distance < 0.8f && raycastHitUp.distance > 0.0f && velocity.y > 0.0f)
        {
            newVelocity.y = 0.0f;
            verticalSpeed = 0.0f;
        }
        if (raycastHitLeft.distance < 0.3f && raycastHitLeft.distance > 0.0f && velocity.x < 0.0f)
        {
            newVelocity.x = 0.0f;
            horizontalSpeed = 0.0f;
        }
        if (raycastHitDown.distance < 1.1f && raycastHitDown.distance > 0.0f && velocity.y < 0.0f)
        {
            newVelocity.y = 0.0f;
            verticalSpeed = 0.0f;
        }

        return newVelocity;
    }
}
