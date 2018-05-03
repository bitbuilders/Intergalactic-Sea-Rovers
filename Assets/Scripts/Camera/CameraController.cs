using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform m_target = null;
    [SerializeField] [Range(1.0f, 20.0f)] float m_cameraStiffness = 5.0f;
    [SerializeField] [Range(-10.0f, 10.0f)] float m_distanceFromTarget = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_cameraLead = 5.0f;
    [SerializeField] [Range(1.0f, 25.0f)] float m_shakeAmplitude = 5.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_shakeRate = 5.0f;
    [SerializeField] [Range(0.0f, 1.0f)] float m_shakeTime = 1.0f;
    [SerializeField] bool m_2D = true;

    Controller m_playerController;
    Camera m_camera;
    Vector3 m_actualPosition;
    Vector3 m_shake;
    Vector3 m_offset;
    float m_followRateHoriz = 0.0f;
    float m_followRateVert = 0.0f;

    void Start()
    {
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
            Vector3 nextPosition = Vector3.zero;
            nextPosition = m_target.position + Vector3.back * 10;
            m_followRateHoriz = Mathf.Lerp(m_followRateHoriz, m_playerController.SpeedPercentageHoriz, Time.deltaTime * m_cameraStiffness * Mathf.Abs(m_playerController.SpeedPercentageHoriz));
            m_followRateVert = Mathf.Lerp(m_followRateVert, m_playerController.SpeedPercentageVert, Time.deltaTime * m_cameraStiffness * Mathf.Abs(m_playerController.SpeedPercentageVert));
            Vector3 attemptedPosition = m_playerController.Velocity.normalized;
            float horizModifier = 0.0f;
            float vertModifier = 0.0f;
            if (m_followRateHoriz < 0.0f && m_playerController.SpeedPercentageHoriz > 0.0f)
                horizModifier = m_followRateHoriz + 1.0f / m_playerController.SpeedPercentageHoriz + 1.0f;
            else horizModifier = m_followRateHoriz;
            if (m_followRateVert < 0.0f) vertModifier = Mathf.Abs(m_followRateVert);
            else vertModifier = m_followRateVert;
            attemptedPosition.x *= horizModifier;
            attemptedPosition.y *= vertModifier;
            print("followH: " + m_followRateHoriz + " | actualH: " + m_playerController.SpeedPercentageHoriz
                + " | followV: " + m_followRateVert + " | actualV: " + m_playerController.SpeedPercentageVert);
            nextPosition += attemptedPosition * m_cameraLead;
            m_actualPosition = Vector3.Lerp(m_actualPosition, nextPosition, Time.deltaTime * m_cameraStiffness);
        }
        else
        {
            m_actualPosition = m_target.position + (-m_target.forward * m_distanceFromTarget);

            Quaternion rotation = Quaternion.LookRotation(m_target.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_cameraStiffness);
        }
        
        Vector3 newPosition = m_actualPosition + m_shake;

        transform.position = newPosition;
    }

    public void Shake(float time, float amplitude = 8.0f, float rate = 6.0f)
    {
        m_shakeTime = time;
        m_shakeAmplitude = amplitude;
        m_shakeRate = rate;
        m_shakeTime = Mathf.Clamp01(m_shakeTime);
    }
}
