using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [SerializeField]
    private Transform m_target;
    [SerializeField]
    private Vector3 m_offset;

    private Transform m_transform;

    private void Start()
    {
        m_transform = transform;
    }

    void Update()
    {
        m_transform.position = m_target.position + m_offset;
    }
}