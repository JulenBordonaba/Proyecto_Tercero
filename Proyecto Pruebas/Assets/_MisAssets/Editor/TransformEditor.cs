using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Transform t = (Transform)target;

        if (GUILayout.Button("Add Copy Components"))
        {
            t.gameObject.AddComponent<CopyComponents>();
        }
    }
}
