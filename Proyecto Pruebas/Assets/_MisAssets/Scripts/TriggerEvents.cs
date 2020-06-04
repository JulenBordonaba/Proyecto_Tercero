using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public string targetTag = "Player";

    public UnityEvent onTriggerEnter = new UnityEvent();
    public UnityEvent onTriggerStay = new UnityEvent();
    public UnityEvent onTriggerExit = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(targetTag))
        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
            onTriggerExit.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag))
            onTriggerStay.Invoke();
    }

}
