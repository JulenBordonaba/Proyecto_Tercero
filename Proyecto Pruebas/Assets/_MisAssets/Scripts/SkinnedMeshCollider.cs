using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshCollider : MonoBehaviour
{
    SkinnedMeshRenderer meshRenderer;
    MeshCollider myCollider;

    private void Start()
    {
        myCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        UpdateCollider();
    }

    public void UpdateCollider()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        myCollider.sharedMesh = null;
        myCollider.sharedMesh = colliderMesh;
    }
}
