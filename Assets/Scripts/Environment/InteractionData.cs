using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionData.asset", menuName = "Assets/Interaction/InteractionData", order = 1)]
public class InteractionData : ScriptableObject
{
    public Interactee leftInteractor;
    public Interactee rightInteractor;
    public string dialogFileName;
}
