using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleCurvedTrack : MonoBehaviour
{
    private BezierSpline m_spline;

    private void Start()
    {
        m_spline = GetComponentInParent<BezierSpline>();
    }

    private void OnTriggerEnter(Collider other)
    {
        FourWheelDrive player;
        if (player = other.GetComponent<FourWheelDrive>())
            player.SetFollowCurvedTrack(m_spline, true);
    }

    private void OnTriggerExit(Collider other)
    {
        FourWheelDrive player;
        if (player = other.GetComponent<FourWheelDrive>())
            player.SetFollowCurvedTrack(m_spline, false);
    }
}