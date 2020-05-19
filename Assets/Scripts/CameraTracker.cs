using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [SerializeField]
    private Transform m_target;
    [SerializeField]
    private Vector3 m_offset;
    [SerializeField]
    private float m_cameraLerpSpeed = 0.2f;
    [SerializeField]
    private float m_cameraSpeedDistancingCoefficient = 15f;

    private Transform m_transform;
    private Vector3 m_lastTargetPosition;

    private void Start()
    {
        m_transform = transform;
        m_lastTargetPosition = m_target.position;
    }

    void FixedUpdate()
    {
        // TODO: move camera down based on distance from ground (to see where landing)
        Vector3 extraCameraDistance = -(Vector3.forward * (m_target.position - m_lastTargetPosition).magnitude * m_cameraSpeedDistancingCoefficient);
        Vector3 newPos = m_target.position + m_offset + extraCameraDistance;
        m_transform.position = Vector3.Lerp(m_transform.position, newPos, m_cameraLerpSpeed);

        m_lastTargetPosition = m_target.position;
    }
}