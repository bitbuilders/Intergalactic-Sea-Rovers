using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData.asset", menuName = "Assets/Event/EventData", order = 1)]
public class EventData : ScriptableObject
{
    public List<EventAction> actions;
}
