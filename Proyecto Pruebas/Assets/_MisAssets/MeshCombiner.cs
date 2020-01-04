using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [ContextMenu("Combine Meshes")]
    public void CombineMeshes()
    {
        Mesh combineTarget = new Mesh();
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        combineTarget.CombineMeshes(combine);
        if (GetComponent<MeshFilter>())
        {
            GetComponent<MeshFilter>().sharedMesh = combineTarget;
            gameObject.SetActive(true);

            transform.localScale = new Vector3(1, 1, 1);
            transform.rotation = Quaternion.identity;
            transform.position = Vector3.zero;
        }

    }
}
