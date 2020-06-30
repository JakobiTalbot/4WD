using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierSpline : MonoBehaviour
{
    [SerializeField]
    private Vector3[] m_points;
    [SerializeField]
    private BezierControlPointMode[] m_modes;
    public int controlPointCount
    {
        get
        {
            return m_points.Length;
        }
    }
    public int curveCount
    {
        get
        {
            return (m_points.Length - 1) / 3;
        }
    }

    public Vector3 GetControlPoint(int index)
    {
        return m_points[index];
    }

    public void SetControlPoint(int index, Vector3 point)
    {
        if (index % 3 == 0)
        {
            Vector3 delta = point - m_points[index];
            if (index > 0)
            {
                m_points[index - 1] += delta;
            }
            if (index + 1 < m_points.Length)
            {
                m_points[index + 1] += delta;
            }
        }
        m_points[index] = point;
        EnforceMode(index);
    }

    public void Reset()
    {
        m_points = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
        m_modes = new BezierControlPointMode[]
        {
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };
    }

    /// <summary>
    /// Returns position at t in world coordinates.
    /// </summary>
    /// <param name="t">How far along the curve to return position at (0-1).</param>
    /// <returns>Position at t along the curve.</returns>
    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = m_points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * curveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetPoint(m_points[i], m_points[i + 1], m_points[i + 2], m_points[i + 3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = m_points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * curveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetFirstDerivative(m_points[i], m_points[i + 1], m_points[i + 2], m_points[i + 3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public BezierControlPointMode GetControlPointMode(int index)
    {
        return m_modes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        m_modes[(index + 1) / 3] = mode;
        EnforceMode(index);
    }

    public void AddCurve()
    {
        Vector3 point = m_points[m_points.Length - 1];
        Array.Resize(ref m_points, m_points.Length + 3);
        point.x += 1f;
        m_points[m_points.Length - 3] = point;
        point.x += 1f;
        m_points[m_points.Length - 2] = point;
        point.x += 1f;
        m_points[m_points.Length - 1] = point;

        Array.Resize(ref m_modes, m_modes.Length + 1);
        m_modes[m_modes.Length - 1] = m_modes[m_modes.Length - 2];
        EnforceMode(m_points.Length - 4);
    }

    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = m_modes[modeIndex];
        if (mode == BezierControlPointMode.Free || modeIndex == 0 || modeIndex == m_modes.Length - 1)
            return;

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            enforcedIndex = middleIndex + 1;
        }
        else
        {
            fixedIndex = middleIndex + 1;
            enforcedIndex = middleIndex - 1;
        }

        Vector3 middle = m_points[middleIndex];
        Vector3 enforcedTangent = middle - m_points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, m_points[enforcedIndex]);
        }
        m_points[enforcedIndex] = middle + enforcedTangent;
    }

    public float ProjectPoint(Vector3 point)
    {
        float shortestDist = Vector3.Distance(GetPoint(0f), point);
        float dist;
        float f = 0;

        // find closest starting point
        for (int i = 1; i < m_points.Length; ++i)
        {
            float t = (float)i / m_points.Length;
            dist = Vector3.Distance(GetPoint(t), point);

            if (dist < shortestDist) // else break?
            {
                shortestDist = dist;
                f = t;
            }
        }

        // refine between closest starting point and next points
        float interval = 0.5f / m_points.Length;
        while (interval > 0.001f)
        {
            dist = Vector3.Distance(GetPoint(f + interval), point);
            if (dist < shortestDist)
            {
                dist = shortestDist;
                f += interval;
            }
            else if ((dist = Vector3.Distance(GetPoint(f - interval), point)) < shortestDist)
            {
                dist = shortestDist;
                f -= interval;
            }
            interval /= 2f;
        }

        return f;
    }
}