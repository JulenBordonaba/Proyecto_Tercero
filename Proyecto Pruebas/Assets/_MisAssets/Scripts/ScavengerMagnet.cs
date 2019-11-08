using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerMagnet : MonoBehaviour
{
    [Tooltip("Pon la fuerza del imán")]
    public float magnetForce;

    public bool inverted = false;
    public bool inUse = false;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (inUse)
        {
            if (other.tag == "Nave")
            {
                print(other.gameObject.name);
                if (other.GetComponentInParent<Rigidbody>())
                {
                    Vector3 direction = GetComponentInParent<NaveController>().modelTransform.position - other.GetComponentInParent<Rigidbody>().transform.position;
                    other.GetComponent<Rigidbody>().AddForce(direction.normalized * magnetForce * (inverted ? -1 : 1));
                }
            }
        }
    }
}
