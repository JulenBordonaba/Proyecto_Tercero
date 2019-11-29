using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteComponentInChildren : MonoBehaviour
{
    [ContextMenu("Delete Colliders")]
    public void DeleteColliders()
    {
        foreach(Collider c in GetComponentsInChildren<Collider>())
        {
            DestroyImmediate(c);
        }
    }
}
