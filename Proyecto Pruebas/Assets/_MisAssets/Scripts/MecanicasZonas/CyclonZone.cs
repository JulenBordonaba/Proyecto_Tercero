using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclonZone : MonoBehaviour
{

    public Vector3 direction;
    public float windForce = 10;

    private List<Rigidbody> objectsInArea = new List<Rigidbody>();

    [Header("Orientación")]
    public bool inverseForward = false;


    private void Start()
    {
        transform.SetParent(null);
        OrientWindToForward();
    }

    private void FixedUpdate()
    {
        foreach(Rigidbody rb in objectsInArea)
        {
            rb.AddForce(direction.normalized * windForce* Time.fixedDeltaTime,ForceMode.VelocityChange);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NaveCentre"))
        {
            objectsInArea.Add(other.gameObject.GetComponentInParent<Rigidbody>());
            other.gameObject.GetComponentInParent<NaveController>().winds.Add(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInArea.Contains(other.gameObject.GetComponentInParent<Rigidbody>()))
        {
            objectsInArea.Remove(other.gameObject.GetComponentInParent<Rigidbody>());
            other.gameObject.GetComponentInParent<NaveController>().winds.Remove(this);
        }
    }

    public void OrientWindToForward()
    {
        direction = transform.forward * (inverseForward? -1 : 1);
    }
}
