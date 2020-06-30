using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Vector3[] m_points;

    public void Reset()
    {
        m_points = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
    }

    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(m_points[0], m_points[1], m_points[2], m_points[3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        return transform.TransformPoint(Bezier.GetFirstDerivative(m_points[0], m_points[1], m_points[2], m_points[3], t)) - transform.position;
    }

    public Vector3 GetDirection (float t)
    {
        return GetVelocity(t).normalized;
    }
}