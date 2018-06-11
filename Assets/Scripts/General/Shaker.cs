using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : Singleton<Shaker>
{
    public void ShakeObject(GameObject obj, float duration, float amplitude, float rate)
    {
        StartCoroutine(Shake(obj, duration, amplitude, rate));
    }

    private IEnumerator Shake(GameObject obj, float dur, float amp, float rate)
    {
        Vector3 startPos = obj.transform.position;
        float time = Mathf.Clamp01(dur);
        for (float i = time; i >= 0.0f; i -= Time.deltaTime)
        {
            float t = Time.time * rate;
            Vector3 shake = Vector2.zero;
            shake.x = i * amp * ((Mathf.PerlinNoise(t, 0.0f) * 2.0f) - 1.0f);
            shake.y = i * amp * ((Mathf.PerlinNoise(0.0f, t) * 2.0f) - 1.0f);
            obj.transform.position = startPos + shake;
            yield return null;
        }

        obj.transform.position = startPos;
    }
}
