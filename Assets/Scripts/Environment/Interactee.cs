using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Interactee.asset", menuName = "Assets/Interaction/Interactee", order = 0)]
public class Interactee : ScriptableObject
{
    public SpriteRenderer sprite;
    public string entityName;
}
