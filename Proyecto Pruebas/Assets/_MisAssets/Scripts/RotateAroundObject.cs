using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateAroundObject : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject target;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);
        transform.Translate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
