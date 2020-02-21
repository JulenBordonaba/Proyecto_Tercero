using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CopyComponents))]
public class CopyComponentsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CopyComponents cc = (CopyComponents)target;

        if(GUILayout.Button("Copy Components"))
        {
            cc.Copy();
            DestroyImmediate(cc);
            EditorGUIUtility.ExitGUI();
        }
    }
}
