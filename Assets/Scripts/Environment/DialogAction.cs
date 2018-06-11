﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogAction.asset", menuName = "Assets/Interaction/DialogAction", order = 3)]
public class DialogAction : ScriptableObject
{
    public enum EntryPoint
    {
        NONE,
        LEFT,
        RIGHT,
        LEFT_START,
        RIGHT_START
    }
    public float shakeAmplitude;
    public float shakeRate;
    public float shakeDuration;
    public float blinkSize;
    public float blinkDuration;
    public float textSpeed;
    public float textAnimationSpeed;
    public EntryPoint entryPoint;
    public string text;
    public Interactee interactee;
}