using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Interactee.asset", menuName = "Assets/Interaction/Interactee", order = 0)]
public class Interactee : ScriptableObject
{
    public Sprite sprite;
    public string entityName;
    public Color color;
    [Range(-3.0f, 3.0f)] public float pitch;
}
