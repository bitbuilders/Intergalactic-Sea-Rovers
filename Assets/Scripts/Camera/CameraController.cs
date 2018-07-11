using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public Transform m_target = null;
    [SerializeField] [Range(0.0f, 900.0f)] float m_rotationSpeed = 90.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_cameraStiffness = 5.0f;
    [SerializeField] [Range(-10.0f, 10.0f)] float m_distanceFromTarget = 5.0f;
    [SerializeField] [Range(-10.0f, 10.0f)] float m_heightFromTarget = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_cameraLead = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_closeDistance = 2.0f;
    [SerializeField] [Range(0.0f, 60.0f)] float m_fovZoom = 20.0f;
    [SerializeField] [Range(0.0f, 25.0f)] float m_shakeAmplitude = 5.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_shakeRate = 5.0f;
    [SerializeField] [Range(0.0f, 1.0f)] float m_shakeTime = 1.0f;
    [SerializeField] bool m_2D = true;
    [SerializeField] bool m_constantShake = false;
    [SerializeField] public bool m_locked = false;

    Entity m_player;
    Camera m_camera;
    Vector3 m_actualPosition;
    Vector3 m_shake;
    Vector3 m_offset;
    Vector3 m_rotation;
    Quaternion m_startingRot;

    void Awake()
    {
        if (m_2D)
        {
            m_target = FindObjectOfType<Player>().transform;
            m_player = m_target.GetComponent<Entity>();
        }
        m_camera = GetComponent<Camera>();
        m_actualPosition = transform.position;
        m_shake = Vector3.zero;
        if (m_2D) m_camera.orthographic = true;
        if (!m_2D) m_camera.orthographic = false;

        m_offset = new Vector3(0.0f, 1.0f, 2.0f).normalized;
    }
    
    void Update()
    {
        if (!m_constantShake)
            m_shakeTime -= Time.deltaTime;
        m_shakeTime = Mathf.Clamp01(m_shakeTime);
        
        float t = Time.time * m_shakeRate;
        m_shake.x = m_shakeTime * m_shakeAmplitude * ((Mathf.PerlinNoise(t, 0.0f) * 2.0f) - 1.0f);
        m_shake.y = m_shakeTime * m_shakeAmplitude * ((Mathf.PerlinNoise(0.0f, t) * 2.0f) - 1.0f);

        bool p1Locked = Input.GetButton(m_player.PlayerNumber + "_Target");
        bool p2Locked = Input.GetAxis(m_player.PlayerNumber + "_Target") != 0.0f;
        m_locked = p1Locked || p2Locked;

        if (!m_locked)
        {
            Vector3 offset = m_offset * m_distanceFromTarget;
            m_rotation.y += Input.GetAxis(m_player.PlayerNumber + "_CamHorizontal") * Time.deltaTime * m_rotationSpeed;
            m_rotation.x += Input.GetAxis(m_player.PlayerNumber + "_CamVertical") * Time.deltaTime * m_rotationSpeed;
            Quaternion rotation = Quaternion.Euler(m_rotation);
            Vector3 newPos = rotation * offset + m_player.Controller.transform.position + Vector3.up * m_heightFromTarget;
            m_rotation.x = Mathf.Clamp(m_rotation.x, -30.0f, 25.0f);
            m_actualPosition = Vector3.Lerp(m_actualPosition, newPos, Time.deltaTime * m_cameraStiffness);
        }
        else
        {
            Vector3 offset = -m_player.transform.forward * m_distanceFromTarget + Vector3.up * (m_heightFromTarget + 1.0f);
            Vector3 newPos = offset + m_player.transform.position;
            m_actualPosition = Vector3.Lerp(m_actualPosition, newPos, Time.deltaTime * m_cameraStiffness);
            m_rotation = (m_startingRot * transform.rotation).eulerAngles;
            m_rotation.x *= -1.0f;
        }
    }

    private void LateUpdate()
    {
        if (m_2D)
        {
            Vector3 offset = Vector3.zero;
            offset.x = m_player.Controller.SpeedPercentageHoriz * m_cameraLead;
            offset.y = m_player.Controller.SpeedPercentageVert * m_cameraLead;
            Vector3 nextPosition = Vector3.zero;
            nextPosition = m_target.transform.position + offset;
            nextPosition.z = -10.0f;
            m_actualPosition = Vector3.Lerp(m_actualPosition, nextPosition, Time.deltaTime * m_cameraStiffness);
        }
        else
        {
            if (!m_locked)
            {
                Quaternion rot = Quaternion.LookRotation(m_player.transform.position + Vector3.up * 0.5f - transform.position, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * m_cameraStiffness * 4.0f);
            }
            else
            {
                Quaternion rot = Quaternion.LookRotation(m_target.transform.position + Vector3.up * 0.5f - transform.position, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * m_cameraStiffness * 4.0f);
            }
        }
        
        Vector3 newPosition = m_actualPosition + (transform.rotation * m_shake);

        transform.position = newPosition;
    }

    public void Entity(Entity entity)
    {
        m_player = entity;

        m_offset.z = -m_player.transform.forward.z * m_offset.z;
        m_startingRot = m_player.transform.rotation;
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
