using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CyclonZone))]

public class CyclonZoneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CyclonZone cyclonZone = (CyclonZone)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Orient Wind To Forward"))
        {
            cyclonZone.OrientWindToForward();
        }


    }
}
