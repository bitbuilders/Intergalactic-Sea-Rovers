using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionData.asset", menuName = "Assets/Interaction/InteractionData", order = 1)]
public class InteractionData : ScriptableObject
{
    public enum InteractionType
    {
        EVENT,
        CONVERSATION,
        MONOLOG
    }
    public Interactee firstInteractor;
    public Interactee secondInteractor;
    public InteractionType interactionType;
    public string dialogFileName;
}
