using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyComponents : MonoBehaviour
{
    public GameObject target;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Copy()
    {
        CopySpecialComponents(target, gameObject);
    }

    private void CopySpecialComponents(GameObject _sourceGO, GameObject _targetGO)
    {
        foreach (var component in _sourceGO.GetComponents<Component>())
        {
            var componentType = component.GetType();
            if (componentType != typeof(Transform) &&
                componentType != typeof(MeshFilter) &&
                componentType != typeof(MeshRenderer)
                )
            {
                Debug.Log("Found a component of type " + component.GetType());
                UnityEditorInternal.ComponentUtility.CopyComponent(component);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(_targetGO);
                Debug.Log("Copied " + component.GetType() + " from " + _sourceGO.name + " to " + _targetGO.name);
            }
        }

    }
}
