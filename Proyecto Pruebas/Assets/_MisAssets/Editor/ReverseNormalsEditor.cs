using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ReverseNormals))]
public class ReverseNormalsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ReverseNormals rn = (ReverseNormals)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Reverse Normals"))
        {
            rn.ReverseNormal();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Reverse Polygons"))
        {
            rn.ReversePolygons();
        }



    }
}
