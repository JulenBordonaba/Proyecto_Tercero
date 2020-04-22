using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{

    public Transform target;

    Camera targetCamera;
    Camera myCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (!target) return;
        if(GetComponent<Camera>())
        {
            myCamera = GetComponent<Camera>();
        }

        if (target.GetComponent<Camera>())
        {
            targetCamera = target.GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) return;

        transform.position = target.position;
        transform.rotation = target.rotation;

        if(myCamera && targetCamera)
        {
            myCamera.fieldOfView = targetCamera.fieldOfView;
        }

    }
}
