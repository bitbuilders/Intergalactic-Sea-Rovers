using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public bool CanMove { get; set; }
    [SerializeField] public Interactee m_interacteeInfo = null;
}
