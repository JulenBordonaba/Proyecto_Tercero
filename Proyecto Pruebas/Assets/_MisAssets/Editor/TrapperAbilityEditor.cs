using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrapperAbility))]
public class TrapperAbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TrapperAbility t = (TrapperAbility)target;

        if(GUILayout.Button("Create Fire Objects"))
        {
            CreateFire(t);
        }

    }

    public void CreateFire(TrapperAbility t)
    {
        foreach (GameObject go in t.fireList)
        {
            t.StartCoroutine(DestroyObject(go));
        }
        t.fireList.Clear();
        float _duration = t.duration;
        while (_duration > 0)
        {
            GameObject newFire = PrefabUtility.InstantiatePrefab(t.firePrefab) as GameObject;
            newFire.transform.parent = t.fireParent;
            newFire.transform.localPosition = Vector3.zero;
            t.fireList.Add(newFire);
            newFire.SetActive(false);
            _duration -= t.instanceRate;
        }
    }

    IEnumerator DestroyObject(GameObject _destroyObject)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(_destroyObject);
    }
}
