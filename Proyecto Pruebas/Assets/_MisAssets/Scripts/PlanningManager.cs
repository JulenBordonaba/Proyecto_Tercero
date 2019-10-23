using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningManager : MonoBehaviour
{
    [Tooltip("pon la máxima velocidad de caída")]
    public float maxFallVelocity;   //velocidad de caída máxima


    private Rigidbody rb;   //rigidbody de la nave

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<NaveManager>().isPlanning)
        {
            LimitateFallVelocity();
        }
    }
    
    private void LimitateFallVelocity()
    {
        if(rb.velocity.y<-maxFallVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, -maxFallVelocity, rb.velocity.z);
        }
    }
    
}
