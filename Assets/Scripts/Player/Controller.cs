using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 100.0f)] protected float m_speed = 5.0f;

    public abstract void Move();
}
