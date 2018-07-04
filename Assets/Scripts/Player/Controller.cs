using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 100.0f)] protected float m_speed = 5.0f;

    protected Vector3 m_velocity;
    protected float m_speedPercentageHoriz;
    protected float m_speedPercentageVert;
    public Vector3 Velocity { get { return m_velocity; } protected set { m_velocity = value; } }
    public float SpeedPercentageHoriz { get { return m_speedPercentageHoriz; } protected set { m_speedPercentageHoriz = value; } }
    public float SpeedPercentageVert { get { return m_speedPercentageVert; } protected set { m_speedPercentageVert = value; } }


    public abstract void Move();
    public abstract void PhysicsUpdate();
}
