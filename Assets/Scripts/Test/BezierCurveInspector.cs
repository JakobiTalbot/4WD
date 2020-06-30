using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
    private BezierCurve m_curve;
    private Transform m_handleTransform;
    private Quaternion m_handleRotation;

    private const int m_lineSteps = 10;
    private const float m_directionScale = 0.5f;

    private void OnSceneGUI()
    {
        m_curve = target as BezierCurve;
        m_handleTransform = m_curve.transform;
        m_handleRotation = Tools.pivotRotation == PivotRotation.Local ? m_handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        Handles.color = Color.grey;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);

        ShowDirections();
        Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = m_curve.GetPoint(0f);
        Handles.DrawLine(point, point + m_curve.GetDirection(0f) * m_directionScale);

        for (int i = 1; i <= m_lineSteps; ++i)
        {
            point = m_curve.GetPoint(i / (float)m_lineSteps);
            Handles.DrawLine(point, point + m_curve.GetDirection(i / (float)m_lineSteps) * m_directionScale);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = m_handleTransform.TransformPoint(m_curve.m_points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, m_handleRotation);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(m_curve, "Move Point");
            EditorUtility.SetDirty(m_curve);
            m_curve.m_points[index] = m_handleTransform.InverseTransformPoint(point);
        }

        return point;
    }
}