using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclonZone : MonoBehaviour
{

    public Vector3 direction;
    public float windForce = 10;

    private List<Rigidbody> objectsInArea = new List<Rigidbody>();
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        foreach(Rigidbody rb in objectsInArea)
        {
            rb.AddForce(direction.normalized * windForce*0.1f,ForceMode.VelocityChange);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NaveCentre"))
        {
            objectsInArea.Add(other.gameObject.GetComponentInParent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInArea.Contains(other.gameObject.GetComponentInParent<Rigidbody>()))
        {
            objectsInArea.Remove(other.gameObject.GetComponentInParent<Rigidbody>());
        }
    }
}
