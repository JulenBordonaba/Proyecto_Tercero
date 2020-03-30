using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor;

public class TrapperAbility : PlayerAbility
{
    //public string firePrefabName;
    public GameObject firePrefab;
    public Transform raySpawn;
    public float maxRayDistance;
    public float instanceRate = 0.1f;
    public float duration = 3f;
    public LayerMask hitLayers;
    public Transform fireParent;
    public List<GameObject> fireList = new List<GameObject>();

    private Coroutine castFire;

    private void Start()
    {
        PrepareFires();
    }

    void PrepareFires()
    {
        foreach(GameObject go in fireList)
        {
            go.transform.SetParent(null);
            go.transform.position = Vector3.zero;
        }
    }

    


    

    IEnumerator CastFire(float loopTime)
    {
        while(true)
        {
            Ray ray = new Ray();
            ray.origin = raySpawn.position;
            ray.direction = -Vector3.up;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxRayDistance, hitLayers))
            {
                photonView.RPC("SetFire", PhotonTargets.All, hit.point);
            }
            yield return new WaitForSeconds(instanceRate);
        }
        
    }

    [PunRPC]
    void SetFire(Vector3 point)
    {
        GameObject fire = GetHiddenFire();
        if (!fire) return;
        fire.SetActive(true);
        fire.transform.position = point;
        fire.transform.forward = new Vector3(raySpawn.forward.x, 0, raySpawn.forward.z);
    }

    GameObject GetHiddenFire()
    {
        foreach(GameObject go in fireList)
        {
            if(!go.activeInHierarchy)
            {
                return go;
            }
        }
        return null;
    }

    IEnumerator StopFire()
    {
        yield return new WaitForSeconds(duration);
        StopCoroutine(castFire);
    }

    public override void Use(bool _forced)
    {
        base.Use(_forced);
        if(!inCooldown)
        {
            inCooldown = true;
            castFire = StartCoroutine(CastFire(instanceRate));
            StartCoroutine(StopFire());
            StartCoroutine(Cooldown(cooldown * (_forced ? 1.5f : 1f)));
        }
    }

}
