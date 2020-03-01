using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public Portal[] portals;
    public List<TipoCombustible> tiposCombustible = new List<TipoCombustible>();
    public List<Material> fuelColors = new List<Material>();
    public float resetTime = 3f;
    

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ResetPortals", 0f, resetTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ResetPortals()
    {
        if (portals.Length % 2 != 0)
        {
            Debug.LogError("Tiene que haber un número par de portales para poder emparejarlos");
            return;
        }
        List<Portal> _portals = new List<Portal>(portals);
        for (int i = 0; i < portals.Length / 2; i++)
        {
            Portal p1 = _portals[UnityEngine.Random.Range(0, _portals.Count - 1)];
            Portal p2 = null;
            do
            {
                p2 = _portals[UnityEngine.Random.Range(0, _portals.Count - 1)];
            } while (p2 == null || p2 == p1);
            p1.pair = p2;
            p2.pair = p1;
            TipoCombustible t = Global.RandomEnumValue<TipoCombustible>();
            p1.tipoCombustible = t;
            p2.tipoCombustible = t;
            Material m = GetMaterial(t);
            p1.SetMaterial(m);
            p2.SetMaterial(m);
            _portals.Remove(p1);
            _portals.Remove(p2);
        }

    }

    public Material GetMaterial(TipoCombustible t)
    {
        for (int i = 0; i < tiposCombustible.Count; i++)
        {
            if (tiposCombustible[i] == t)
            {
                return fuelColors[i];
            }
        }
        return fuelColors[0];
    }
    

}