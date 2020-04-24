using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateDistance : MonoBehaviour
{
    public Transform startPoint;
    public Transform finishPoint;

    

    public float Distance
    {
        get { return Vector3.Distance(startPoint.position, finishPoint.position); }
    }
}
