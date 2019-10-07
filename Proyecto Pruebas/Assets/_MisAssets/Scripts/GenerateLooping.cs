using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLooping : MonoBehaviour
{
    public GameObject plane;
    public Transform middle;
    public int definition = 180;

    [ContextMenu("Generate Looping")]
    public void CreateLooping()
    {
        for(int i=0;i<definition;i++)
        {
            Instantiate(plane, middle).transform.rotation=Quaternion.Euler((float)((360/definition)*i),0f,0f);
        }
    }
}
