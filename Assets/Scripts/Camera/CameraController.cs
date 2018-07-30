using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraController : MonoBehaviour
{
    [SerializeField] public Transform m_target = null;
    [SerializeField] [Range(0.0f, 900.0f)] float m_rotationSpeed = 90.0f;
    [SerializeField] [Range(0.0f, 900.0f)] float m_drunkRotationSpeed = 135.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_cameraStiffness = 5.0f;
    [SerializeField] [Range(-10.0f, 10.0f)] float m_distanceFromTarget = 5.0f;
    [SerializeField] [Range(-10.0f, 10.0f)] float m_heightFromTarget = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_cameraLead = 5.0f;
    [SerializeField] [Range(0.0f, 25.0f)] float m_shakeAmplitude = 5.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_shakeRate = 5.0f;
    [SerializeField] [Range(0.0f, 1.0f)] float m_shakeTime = 1.0f;
    [SerializeField] bool m_2D = true;
    [SerializeField] bool m_constantShake = false;
    [SerializeField] public bool m_locked = false;
    [SerializeField] PostProcessingProfile m_drunkProfile;
    [SerializeField] LayerMask m_groundMask = 0;

    PostProcessingBehaviour m_processingBehaviour;
    PostProcessingProfile m_normalProfile;
    Entity m_player;
    Camera m_camera;
    Vector3 m_actualPosition;
    Vector3 m_shake;
    Vector3 m_offset;
    Vector3 m_rotation;
    Quaternion m_startingRot;
    float m_baseDistanceFromTarget;
    float m_startAmplitude;
    float m_startRate;
    float m_responsiveness;
    bool m_p2ResetLock;

    void Awake()
    {
        if (m_2D)
        {
            m_target = FindObjectOfType<Player>().transform;
            m_player = m_target.GetComponent<Entity>();
        }
        m_camera = GetComponent<Camera>();
        m_processingBehaviour = GetComponent<PostProcessingBehaviour>();
        m_normalProfile = m_processingBehaviour.profile;
        m_actualPosition = transform.position;
        m_shake = Vector3.zero;
        if (m_2D) m_camera.orthographic = true;
        if (!m_2D) m_camera.orthographic = false;

        m_offset = new Vector3(0.0f, 1.0f, 2.0f).normalized;
        m_baseDistanceFromTarget = m_distanceFromTarget;
        m_startAmplitude = m_shakeAmplitude;
        m_startRate = m_shakeRate;
        m_p2ResetLock = false;
        m_responsiveness = m_cameraStiffness;
    }
    
    void Update()
    {
        if (!m_constantShake)
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
            offset.x = m_player.Controller.SpeedPercentageHoriz * m_cameraLead;
            offset.y = m_player.Controller.SpeedPercentageVert * m_cameraLead;
            Vector3 nextPosition = Vector3.zero;
            nextPosition = m_target.transform.position + offset;
            nextPosition.z = -10.0f;
            m_actualPosition = Vector3.Lerp(m_actualPosition, nextPosition, Time.deltaTime * m_responsiveness);
        }
        else
        {
            bool p1Locked = Input.GetButtonDown(m_player.PlayerNumber + "_Target");
            bool reset = Input.GetAxis(m_player.PlayerNumber + "_Target") == 0.0f;
            if (reset)
                m_p2ResetLock = true;

            bool p2Locked = Input.GetAxis(m_player.PlayerNumber + "_Target") != 0.0f && m_p2ResetLock;

            if (p1Locked || p2Locked)
            {
                m_locked = !m_locked;
                m_p2ResetLock = false;
            }
            if (m_player.Drunk)
            {
                m_responsiveness = m_cameraStiffness * 0.5f;
            }
            else
            {
                m_responsiveness = m_cameraStiffness;
            }

            if (!m_locked)
            {
                Vector3 offset = m_offset * m_distanceFromTarget;
                float rotSpeed = (m_player.Drunk) ? m_drunkRotationSpeed : m_rotationSpeed;
                m_rotation.y += Input.GetAxis(m_player.PlayerNumber + "_CamHorizontal") * Time.deltaTime * rotSpeed;
                m_rotation.x += Input.GetAxis(m_player.PlayerNumber + "_CamVertical") * Time.deltaTime * rotSpeed;
                Quaternion rotation = Quaternion.Euler(m_rotation);
                Vector3 newPos = rotation * offset + m_player.transform.position + Vector3.up * m_heightFromTarget;
                m_rotation.x = Mathf.Clamp(m_rotation.x, -30.0f, 25.0f);
                m_actualPosition = Vector3.Lerp(m_actualPosition, newPos, Time.deltaTime * m_responsiveness);
            }
            else
            {
                Vector3 offset = -m_player.transform.forward * m_distanceFromTarget + Vector3.up * (m_heightFromTarget + 1.0f);
                Vector3 newPos = offset + m_player.transform.position;
                m_actualPosition = Vector3.Lerp(m_actualPosition, newPos, Time.deltaTime * m_responsiveness);
                m_rotation = (m_startingRot * transform.rotation).eulerAngles;
                m_rotation.x = -10.0f;
            }

            if (!m_locked)
            {
                Quaternion rot = Quaternion.LookRotation(m_player.Controller.transform.position + Vector3.up * 0.5f - transform.position, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * m_responsiveness * 3.0f);
            }
            else
            {
                Quaternion rot = Quaternion.LookRotation(m_target.transform.position + Vector3.up * 0.5f - transform.position, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * m_responsiveness * 3.0f);
            }
        }

        RaycastHit raycastHit;
        Vector3 dir = m_actualPosition - (m_player.transform.position + Vector3.up);
        Ray r = new Ray(m_player.transform.position + Vector3.up * m_heightFromTarget, dir.normalized);

        if (Physics.Raycast(r, out raycastHit, m_baseDistanceFromTarget, m_groundMask))
        {
            m_distanceFromTarget = raycastHit.distance;
        }
        else
        {
            m_distanceFromTarget = m_baseDistanceFromTarget;
        }

        Vector3 newPosition = m_actualPosition + (transform.rotation * m_shake);
        transform.position = newPosition;
    }

    public void Entity(Entity entity)
    {
        m_player = entity;

        m_offset.z = -m_player.transform.forward.z * m_offset.z;
        m_startingRot = m_player.transform.rotation;
        m_rotation = Vector3.zero;
        m_rotation.x = -15.0f;
        m_rotation = m_startingRot * m_rotation;
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

    public void Drunk(float duration, float drunkiness)
    {
        m_shakeAmplitude = m_startAmplitude * drunkiness;
        m_shakeRate = m_startRate;
        m_processingBehaviour.profile = m_drunkProfile;
        StopAllCoroutines();
        StartCoroutine(LessDrunk(m_startAmplitude, m_startRate, duration));
    }

    IEnumerator LessDrunk(float startAmp, float startRate, float duration)
    {
        float maxA = m_shakeAmplitude;
        float maxR = m_shakeRate;
        yield return new WaitForSeconds(duration - 1.0f);
        for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime)
        {
            m_shakeAmplitude = Mathf.Lerp(startAmp, maxA, i);
            m_shakeRate = Mathf.Lerp(startRate, maxR, i);
            yield return null;
        }
        m_shakeAmplitude = startAmp;
        m_shakeRate = startRate;
        m_processingBehaviour.profile = m_normalProfile;
    }
}
