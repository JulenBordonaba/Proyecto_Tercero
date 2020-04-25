using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshCombiner))]

public class MeshCombinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MeshCombiner combiner = (MeshCombiner)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Combine"))
        {
            combiner.CombineMeshes();
        }

        //GUILayout.Space(10);
        //if (GUILayout.Button("Copy Material"))
        //{
        //    combiner.CopyMaterial();
        //}

    }
}
