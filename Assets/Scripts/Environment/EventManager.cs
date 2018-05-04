using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [SerializeField] List<EventData> events = null;

    public void PlayEvent(EventData eventData)
    {

    }
}
