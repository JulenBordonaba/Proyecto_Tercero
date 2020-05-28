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

         foreach(Portal p in _portals)
        {
            if(p.animator!=null)
            {
                p.animator.SetTrigger("Deactivate");
            }
            
        }
        StartCoroutine(ChangePortalPairs(_portals));

    }

    IEnumerator ChangePortalPairs(List<Portal> _portals)
    {
        if (_portals.Count <= 0) yield break;
        yield return new WaitForSeconds(_portals[0].activationClip.averageDuration);
        for (int i = 0; i < (_portals.Count); i++)
        {
            if (_portals.Count <= 0) yield break;
            Portal p1 = _portals[UnityEngine.Random.Range(0, _portals.Count)];
            _portals.Remove(p1);
            if (_portals.Count <= 0) yield break ;


            Portal p2 = _portals[UnityEngine.Random.Range(0, _portals.Count)];
            _portals.Remove(p2);


            //print("sale");
            p1.pair = p2;
            p2.pair = p1;
            TipoCombustible t = Global.RandomEnumValue<TipoCombustible>();
            p1.tipoCombustible = t;
            p2.tipoCombustible = t;
            Material m = GetMaterial(t);
            p1.SetMaterial(m);
            p2.SetMaterial(m);
            p1.naves = new List<NaveManager>();
            p2.naves = new List<NaveManager>();


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