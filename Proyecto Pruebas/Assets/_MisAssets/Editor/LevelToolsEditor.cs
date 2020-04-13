using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelTools))]
public class LevelToolsEditor : Editor
{
    GameObject padre;
    string nombreGrupo = "Objetos anteriores";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LevelTools tools = (LevelTools)target;

        GUILayout.Space(10);
        if(GUILayout.Button("Snap Objects"))
        {
            Snap(tools);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Replace Objects"))
        {
            Replace(tools);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Rotate Objects"))
        {
            Rotate(tools);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Scale Objects"))
        {
            Scale(tools);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Make Obstacles"))
        {
            MakeObstacle(tools);
        }

    }

    
    
    public void Replace(LevelTools lt)
    {
        
        padre = new GameObject(nombreGrupo);
        padre.transform.parent = lt.transform;
        

        foreach (GameObject child in GetChildsWithTag(lt))
        {

            GameObject newGO = PrefabUtility.InstantiatePrefab(lt.replaceObject) as GameObject;
            newGO.transform.position = child.transform.position;
            newGO.transform.rotation = child.transform.rotation;
            newGO.transform.SetParent(child.transform.parent);
            child.SetActive(false);
            child.transform.SetParent(padre.transform);
        }
    }

    public void Scale(LevelTools lt)
    {
        foreach (GameObject child in GetChildsWithTag(lt))
        {
            float scaleFactor = Random.Range(lt.minScale, lt.maxScale);
            child.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }

    public void Rotate(LevelTools lt)
    {
        foreach (GameObject child in GetChildsWithTag(lt))
        {
            child.transform.eulerAngles = new Vector3(Random.Range(lt.minXRotation, lt.maxXRotation), Random.Range(lt.minYRotation, lt.maxYRotation), Random.Range(lt.minZRotation, lt.maxZRotation));
        }
    }



    public void Snap(LevelTools lt)
    {
        foreach (GameObject child in GetChildsWithTag(lt))
        {
            SnapObject(lt, child.transform);
        }
    }

    void SnapObject(LevelTools lt, Transform child)
    {
        Ray ray = new Ray();
        ray.origin = child.transform.position;

        ray.direction = -Vector3.up;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, lt.groundLayers))
        {
            child.position = hit.point + new Vector3(0, lt.heightDiference, 0);
        }
    }

    public List<GameObject> GetChildsWithTag(LevelTools lt)
    {
        List<GameObject> childsWithTag = new List<GameObject>();
        Transform[] childs = lt.GetComponentsInChildren<Transform>();
        foreach (Transform child in childs)
        {
            if (child.gameObject.CompareTag(lt.childTag))
            {
                childsWithTag.Add(child.gameObject);
            }
        }
        return childsWithTag;
    }

    public void MakeObstacle(LevelTools lt)
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if (!go.GetComponent<PhotonView>())
            {
                PhotonView pv = go.AddComponent<PhotonView>();
            }
            if (!go.GetComponent<Stats>())
            {
                Stats s = go.AddComponent<Stats>();
                s.collisionDamage = lt.collisionDamage;
            }
            if (!go.GetComponent<DamageManager>())
            {
                DamageManager dm = go.AddComponent<DamageManager>();
                dm.minDamage = lt.minDamage;
            }
        }

    }
}
