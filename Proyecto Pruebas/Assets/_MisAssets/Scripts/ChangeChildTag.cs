using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeChildTag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Change Child Tag")]
    public void ChangeTag()
    {
        Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
        for(int i=0;i<childTransforms.Length;i++)
        {
            childTransforms[i].gameObject.tag = gameObject.tag;
        }
    }
}
