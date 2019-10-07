using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToCamera : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.gameObject.transform);
        transform.Rotate(0, 0, 180f, Space.Self);
    }
}
