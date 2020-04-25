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
        if (GUILayout.Button("Combine Children"))
        {
            combiner.DivideMeshes();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Delete Combined Children"))
        {
            combiner.DeleteCombinedMesh();


        }

        GUILayout.Space(50);
        if (GUILayout.Button("Combine Array"))
        {
            combiner.CombineMeshesArray();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Delete Combined Array"))
        {
            combiner.DeleteCombinedMeshArray();


        }

        //GUILayout.Space(10);
        //if (GUILayout.Button("Copy Material"))
        //{
        //    combiner.CopyMaterial();
        //}

    }
}
