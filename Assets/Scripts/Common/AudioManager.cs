using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] [Range(1, 50)] int m_channelCount = 20;
    [SerializeField] AudioData[] m_audioClips = null;

    AudioSource[] m_loopedSounds;

    static AudioManager ms_instance = null;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (ms_instance == null)
            ms_instance = this;
        else
            Destroy(gameObject);

        m_loopedSounds = new AudioSource[m_channelCount];

        for (int i = 0; i < m_loopedSounds.Length; ++i)
        {
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.playOnAwake = false;
            audio.loop = true;
            audio.rolloffMode = AudioRolloffMode.Linear;
            m_loopedSounds[i] = audio;
        }

        foreach (AudioData clip in m_audioClips)
        {
            if (clip.playOnAwake)
            {
                PlayClip(clip.clipName, clip.startPosition, false, transform, 1.0f);
            }
        }
    }

    public void PlayClip(string clipName, Vector3 position, bool playIndefinitely, Transform parent, float pitch, float duration = 0.0f)
    {
        AudioData data = GetClipFromName(clipName);
        data.pitch = pitch;

        if (data.global)
        {
            PlayLoop(data, position, duration);
        }
        else
        {
            PlaySound(data, position, playIndefinitely, parent, duration);
        }
    }

    private void PlayLoop(AudioData data, Vector3 position, float duration)
    {
        foreach (AudioSource source in m_loopedSounds)
        {
            if (!source.isPlaying)
            {
                PlayAudioFromData(source, data);
                if (duration > 0.0f)
                {
                    StartCoroutine(StopClipAfterTime(duration, source, false));
                }
                break;
            }
        }
    }

    private void PlaySound(AudioData data, Vector3 position, bool playIndefinitely, Transform parent, float duration)
    {
        GameObject obj = new GameObject();
        obj.transform.parent = parent;
        obj.transform.position = position;

        AudioSource audio = obj.AddComponent<AudioSource>();
        audio.rolloffMode = AudioRolloffMode.Linear;
        PlayAudioFromData(audio, data);
        if (!playIndefinitely || !data.loop)
        {
            float stopTime = duration <= 0.0f ? data.clip.length : duration;
            StartCoroutine(StopClipAfterTime(stopTime, audio, true));
        }
    }

    private void PlayAudioFromData(AudioSource source, AudioData data)
    {
        source.Stop();
        source.clip = data.clip;
        source.volume = data.volume;
        source.pitch = data.pitch;
        source.loop = data.loop;
        source.spatialBlend = data.global ? 0.0f : 1.0f;
        source.maxDistance = data.radius;
        source.outputAudioMixerGroup = data.output;
        source.Play();
    }

    private AudioData GetClipFromName(string name)
    {
        AudioData clip = null;

        foreach (AudioData source in m_audioClips)
        {
            if (source.clipName == name)
            {
                clip = source;
            }
        }

        return clip;
    }

    IEnumerator StopClipAfterTime(float duration, AudioSource source, bool destroy)
    {
        yield return new WaitForSeconds(duration);
        if (destroy)
        {
            if (source)
                Destroy(source.gameObject);
        }
        else
        {
            source.Stop();
        }
    }
}
