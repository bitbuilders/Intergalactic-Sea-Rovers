using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 100.0f)] protected float m_speed = 5.0f;

    protected Vector3 m_velocity;
    protected float m_speedPercentage;
    public Vector3 Velocity { get { return m_velocity; } protected set { m_velocity = value; } }
    public float SpeedPercentage { get { return m_speedPercentage; } protected set { m_speedPercentage = value; } }

    public abstract void Move();
}
