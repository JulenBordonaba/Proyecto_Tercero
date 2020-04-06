using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarTarget : MonoBehaviour
{
    public GameObject radarImage;

    public List<Radar> radars = new List<Radar>();
    

    private void OnDisable()
    {
        Disable();
    }

    public void Disable()
    {
        foreach (Radar r in radars)
        {
            r.RemoveRadarTarget(this);
        }
    }
}
