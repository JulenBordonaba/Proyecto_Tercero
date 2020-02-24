using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclonZone : MonoBehaviour
{

    private List<GameObject> objectsInArea = new List<GameObject>();
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NaveCentre"))
        {
            objectsInArea.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInArea.Contains(other.gameObject))
        {
            objectsInArea.Remove(other.gameObject);
        }
    }
}
