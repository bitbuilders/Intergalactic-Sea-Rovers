using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioData", menuName = "Audio/AudioData", order = 1)]
public class AudioData : ScriptableObject
{
    public string clipName;
    public AudioClip clip;
    public Vector3 startPosition;
    public float volume;
    public float pitch;
    public float radius;
    public bool global;
    public bool loop;
    public bool playOnAwake;
    public AudioMixerGroup output;
}
