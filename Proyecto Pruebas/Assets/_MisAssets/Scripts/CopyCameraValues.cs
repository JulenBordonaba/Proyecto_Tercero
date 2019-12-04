using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCameraValues : MonoBehaviour
{
    public Camera targetCamera;

    private Camera myCamera;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = GetComponent<Camera>();   
    }

    // Update is called once per frame
    void Update()
    {
        CopyValues();
    }

    private void CopyValues()
    {
        myCamera.rect = targetCamera.rect;
        myCamera.fieldOfView = targetCamera.fieldOfView;
        transform.rotation = targetCamera.transform.rotation;
    }
}
