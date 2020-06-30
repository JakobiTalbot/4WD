using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor
{
    private BezierSpline m_spline;
    private Transform m_handleTransform;
    private Quaternion m_handleRotation;

    private const int m_stepsPerCurve = 10;
    private const float m_directionScale = 4f;
    private const float m_handleSize = 0.04f;
    private const float m_pickSize = 0.06f;

    private int m_selectedIndex = -1;

    private static Color[] m_modeColours =
    {
        Color.white,
        Color.yellow,
        Color.cyan
    };

    private void OnSceneGUI()
    {
        m_spline = target as BezierSpline;
        m_handleTransform = m_spline.transform;
        m_handleRotation = Tools.pivotRotation == PivotRotation.Local ? m_handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < m_spline.controlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.grey;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }

        ShowDirections();
    }

    public override void OnInspectorGUI()
    {
        m_spline = target as BezierSpline;

        if (m_selectedIndex >= 0 && m_selectedIndex < m_spline.controlPointCount)
        {
            DrawSelectedPointInspector();
        }

        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(m_spline, "Add Curve");
            m_spline.AddCurve();
            EditorUtility.SetDirty(m_spline);
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", m_spline.GetControlPoint(m_selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(m_spline, "Move Point");
            EditorUtility.SetDirty(m_spline);
            m_spline.SetControlPoint(m_selectedIndex, point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", m_spline.GetControlPointMode(m_selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(m_spline, "Change Point Mode");
            m_spline.SetControlPointMode(m_selectedIndex, mode);
            EditorUtility.SetDirty(m_spline);
        }
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = m_spline.GetPoint(0f);
        Handles.DrawLine(point, point + m_spline.GetDirection(0f) * m_directionScale);
        int steps = m_stepsPerCurve * m_spline.curveCount;

        for (int i = 1; i <= steps; ++i)
        {
            point = m_spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + m_spline.GetDirection(i / (float)steps) * m_directionScale);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = m_handleTransform.TransformPoint(m_spline.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        Handles.color = m_modeColours[(int)m_spline.GetControlPointMode(index)];
        if (Handles.Button(point, m_handleRotation, m_handleSize * size, m_pickSize * size, Handles.DotHandleCap))
        {
            m_selectedIndex = index;
            Repaint();
        }

        if (m_selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, m_handleRotation);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_spline, "Move Point");
                EditorUtility.SetDirty(m_spline);
                m_spline.SetControlPoint(index, m_handleTransform.InverseTransformPoint(point));
            }
        }

        return point;
    }
}