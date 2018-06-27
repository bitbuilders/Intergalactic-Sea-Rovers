using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : Controller
{
    [SerializeField] [Range(0.0f, 10.0f)] float friction = 1.0f;
    [SerializeField] [Range(0.0f, 30.0f)] float speedRamp = 2.0f;
    [SerializeField] [Range(0.1f, 30.0f)] float maxSpeed = 5.0f;
    [SerializeField] LayerMask m_barrierMask = 0;
    
    float horizontalSpeed = 0.0f;
    float verticalSpeed = 0.0f;

    public override void Move()
    {
        m_velocity = Vector3.zero;

        float s = Time.deltaTime * speedRamp;
        horizontalSpeed += Input.GetAxis("Horizontal") * s;
        verticalSpeed += Input.GetAxis("Vertical") * s;

        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);
        verticalSpeed = Mathf.Clamp(verticalSpeed, -maxSpeed, maxSpeed);

        m_velocity.x = horizontalSpeed * (1.0f - Mathf.Abs(verticalSpeed) * 0.05f); // Limits velocity if traveling diagonally
        m_velocity.y = verticalSpeed * (1.0f - Mathf.Abs(horizontalSpeed) * 0.05f); // "
        m_velocity *= Time.deltaTime * m_speed;

        if (m_velocity.magnitude > 0.0f)
        {
            m_velocity = LimitVelocity(m_velocity);
        }

        CalculateSpeedPercentage(m_velocity);

        transform.position += m_velocity;

        float f = Time.deltaTime * friction;
        if (horizontalSpeed > 0.05f) horizontalSpeed -= f;
        else if (horizontalSpeed < -0.05f) horizontalSpeed += f;
        else horizontalSpeed = 0.0f;
        if (verticalSpeed > 0.05f) verticalSpeed -= f;
        else if (verticalSpeed < -0.05f) verticalSpeed += f;
        else verticalSpeed = 0.0f;
    }

    public override void PhysicsUpdate()
    {

    }

    private void CalculateSpeedPercentage(Vector2 direction)
    {
        float speed = Time.deltaTime * 1.0f;
        if (direction.x < -0.05f)
        {
            m_speedPercentageHoriz -= speed;
        }
        else if (direction.x > 0.05f)
        {
            m_speedPercentageHoriz += speed;
        }
        else
        {
            if (m_speedPercentageHoriz > 0.05f)
                m_speedPercentageHoriz -= speed * 2.0f;
            else if (m_speedPercentageHoriz < -0.05f)
                m_speedPercentageHoriz += speed * 2.0f;
            else
                m_speedPercentageHoriz = 0.0f;
        }

        if (direction.y < -0.05f)
        {
            m_speedPercentageVert -= speed;
        }
        else if (direction.y > 0.05f)
        {
            m_speedPercentageVert += speed;
        }
        else
        {
            if (m_speedPercentageVert > 0.05f)
                m_speedPercentageVert -= speed * 2.0f;
            else if (m_speedPercentageVert < -0.05f)
                m_speedPercentageVert += speed * 2.0f;
            else
                m_speedPercentageVert = 0.0f;
        }

        m_speedPercentageHoriz = Mathf.Clamp(m_speedPercentageHoriz, -1.0f, 1.0f);
        m_speedPercentageVert = Mathf.Clamp(m_speedPercentageVert, -1.0f, 1.0f);
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
