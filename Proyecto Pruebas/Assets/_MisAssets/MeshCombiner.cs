using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public MeshFilter[] filters;

    public int subObjects = 1;

    public void CombineMeshes()
    {


        Mesh combineTarget = new Mesh();
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        if (!GetComponent<MeshFilter>())
        {
            gameObject.AddComponent<MeshFilter>();
        }

        if (!GetComponent<MeshRenderer>())
        {
            gameObject.AddComponent<MeshRenderer>();
        }

        transform.SetParent(null);
        Vector3 parentPosition = transform.position;
        transform.position = Vector3.zero;

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].transform.position += parentPosition;
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

    public void DivideMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true);

        for (int i = 0; i < subObjects; i++)
        {


            GameObject subObject = null;

            subObject = GetChildByName(gameObject.name + "_" + i);

            if(subObject==null)
            {
                subObject = new GameObject();
            }

            subObject.name = gameObject.name + "_" + i;
            subObject.transform.SetParent(transform);
            subObject.transform.position = Vector3.zero;
            subObject.transform.rotation = Quaternion.identity;
            //if(!subObject.GetComponent<MeshCombiner>())
            //{
            //    subObject.AddComponent<MeshCombiner>();
            //}
        }

        
        int currentObject = 0;

        for(int i=0;i<meshFilters.Length;i++)
        {
            Vector3 scale = meshFilters[i].transform.lossyScale;
            Vector3 position = meshFilters[i].transform.position;
            Quaternion rotation = meshFilters[i].transform.rotation;
            if (currentObject<subObjects-1)
            {
                if(i<(Mathf.FloorToInt(meshFilters.Length/subObjects)*(currentObject+1)))
                {
                    meshFilters[i].transform.SetParent(GetChildByName(gameObject.name + "_" + currentObject).transform);
                }
                else
                {
                    currentObject += 1;
                    meshFilters[i].transform.SetParent(GetChildByName(gameObject.name + "_" + currentObject).transform);
                }
            }
            else
            {
                meshFilters[i].transform.SetParent(GetChildByName(gameObject.name + "_" + currentObject).transform);
            }
            meshFilters[i].transform.position = position;
            meshFilters[i].transform.SetGlobalScale( scale);
            meshFilters[i].transform.rotation = rotation;
        }

        for (int i = 0; i < subObjects; i++)
        {

            CombineInObject(GetChildByName(gameObject.name + "_" + i));

        }

    }



    public void CombineInObject(GameObject _object)
    {
        if (!_object.GetComponent<MeshFilter>())
        {
            _object.AddComponent<MeshFilter>();
        }

        if (!_object.GetComponent<MeshRenderer>())
        {
            _object.AddComponent<MeshRenderer>();
        }

        _object.transform.SetParent(null);
        Vector3 parentPosition = _object.transform.position;
        _object.transform.position = Vector3.zero;

        Mesh combineTarget = new Mesh();
        MeshFilter[] meshFilters = _object.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].transform.position += parentPosition;
            meshFilters[i].gameObject.SetActive(false);
        }


        combineTarget.CombineMeshes(combine);
        if (_object.GetComponent<MeshFilter>())
        {
            _object.GetComponent<MeshFilter>().sharedMesh = combineTarget;
            _object.SetActive(true);

            _object.transform.localScale = new Vector3(1, 1, 1);
            _object.transform.rotation = Quaternion.identity;
            _object.transform.position = Vector3.zero;
        }
    }


    public GameObject GetChildByName(string _name)
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);

        foreach(Transform t in children)
        {
            if (t.gameObject.name == _name) return t.gameObject;
        }
        return null;
        
    }

    public void CombineMeshesArray()
    {

        foreach(MeshFilter f in filters)
        {
            GameObject go = Instantiate(f.gameObject, f.transform.position, f.transform.rotation);
            go.transform.SetParent(transform);
            f.gameObject.SetActive(false);
            StartCoroutine(DestroyOnEndOfFrame(go));
        }


        DivideMeshes();

    }
    

    IEnumerator DestroyOnEndOfFrame(GameObject objectToDestroy)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(objectToDestroy);
    }

    public void DeleteCombinedMesh()
    {
        DestroyImmediate(GetComponent<MeshFilter>());

        foreach (MeshFilter mf in GetComponentsInChildren<MeshFilter>(true))
        {
            mf.gameObject.SetActive(true);
        }
    }

    public void DeleteCombinedMeshArray()
    {
        DestroyImmediate(GetComponent<MeshFilter>());

        foreach (MeshFilter mf in filters)
        {
            mf.gameObject.SetActive(true);
        }
    }
    
}
