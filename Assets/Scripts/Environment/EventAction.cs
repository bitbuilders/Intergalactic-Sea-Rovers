using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventAction.asset", menuName = "Assets/Event/EventAction", order = 0)]
public class EventAction : ScriptableObject
{
    [System.Serializable]
    public enum ActionType
    {
        ANIMATION,
        TEXT,
        DIALOG,
    }
    public ActionType type;
    public SpriteRenderer sprite;
    public string textOrDialog;
    public float duration;
}
