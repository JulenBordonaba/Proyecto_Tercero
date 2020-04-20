using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


[RequireComponent(typeof(MeshFilter))]
public class ReverseNormals : MonoBehaviour
{
    [ContextMenu("Reverse normals")]
    public void ReverseNormal()
    {
        MeshFilter filter = GetComponent(typeof(MeshFilter)) as MeshFilter;
        if (filter != null)
        {
            Mesh mesh = filter.sharedMesh;

            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            mesh.normals = normals;

            for (int m = 0; m < mesh.subMeshCount; m++)
            {
                int[] triangles = mesh.GetTriangles(m);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }
        }
    }


    [ContextMenu("Reverse poligons")]
    public void ReversePolygons()
    {

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}
