﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireWall : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_velocity;

    void FixedUpdate()
    {
        transform.position += m_velocity * Time.fixedDeltaTime;
    }
}