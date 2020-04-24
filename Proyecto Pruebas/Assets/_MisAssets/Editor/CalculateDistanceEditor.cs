using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CalculateDistance))]
public class CalculateDistanceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CalculateDistance calculateDistance = (CalculateDistance)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Print Distance"))
        {
            Debug.Log(calculateDistance.Distance);
        }

    }
}
