using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] Transform m_target = null;
    [SerializeField] [Range(1.0f, 20.0f)] float m_cameraStiffness = 5.0f;
    [SerializeField] [Range(-10.0f, 10.0f)] float m_distanceFromTarget = 5.0f;
    [SerializeField] [Range(-10.0f, 10.0f)] float m_heightFromTarget = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_cameraLead = 5.0f;
    [SerializeField] [Range(1.0f, 25.0f)] float m_shakeAmplitude = 5.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_shakeRate = 5.0f;
    [SerializeField] [Range(0.0f, 1.0f)] float m_shakeTime = 1.0f;
    [SerializeField] bool m_2D = true;

    Controller m_playerController;
    Camera m_camera;
    Vector3 m_actualPosition;
    Vector3 m_shake;

    void Start()
    {
        m_target = FindObjectOfType<Player>().transform;
        m_playerController = m_target.GetComponent<Player>().Controller;
        m_camera = GetComponent<Camera>();
        m_actualPosition = transform.position;
        m_shake = Vector3.zero;
        if (m_2D) m_camera.orthographic = true;
        if (!m_2D) m_camera.orthographic = false;
    }
    
    void Update()
    {
        m_shakeTime -= Time.deltaTime;
        m_shakeTime = Mathf.Clamp01(m_shakeTime);
        
        float t = Time.time * m_shakeRate;
        m_shake.x = m_shakeTime * m_shakeAmplitude * ((Mathf.PerlinNoise(t, 0.0f) * 2.0f) - 1.0f);
        m_shake.y = m_shakeTime * m_shakeAmplitude * ((Mathf.PerlinNoise(0.0f, t) * 2.0f) - 1.0f);
    }

    private void LateUpdate()
    {
        if (m_2D)
        {
            Vector3 offset = Vector3.zero;
            offset.x = m_playerController.SpeedPercentageHoriz * m_cameraLead;
            offset.y = m_playerController.SpeedPercentageVert * m_cameraLead;
            Vector3 nextPosition = Vector3.zero;
            nextPosition = m_target.transform.position + offset;
            nextPosition.z = -10.0f;
            m_actualPosition = Vector3.Lerp(m_actualPosition, nextPosition, Time.deltaTime * m_cameraStiffness);
        }
        else
        {
            Vector3 offset = Vector3.up * m_heightFromTarget;
            Vector3 dirToTarget = m_target.position - m_playerController.transform.position;
            Vector3 newPos = -dirToTarget.normalized * m_distanceFromTarget + m_playerController.transform.position;
            //newPos.y = 0.0f;
            newPos += offset;
            m_actualPosition = Vector3.Lerp(m_actualPosition, newPos, Time.deltaTime * m_cameraStiffness);
            //TODO: Place camera behind player to see target

            Quaternion rotation = Quaternion.LookRotation(m_target.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_cameraStiffness);
        }
        
        Vector3 newPosition = m_actualPosition + m_shake;

        transform.position = newPosition;
    }

    public void Target(Transform target)
    {
        m_target = target;
    }

    public void Shake(float time, float amplitude = 8.0f, float rate = 6.0f)
    {
        m_shakeTime = time;
        m_shakeAmplitude = amplitude;
        m_shakeRate = rate;
        m_shakeTime = Mathf.Clamp01(m_shakeTime);
    }
}
