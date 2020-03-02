using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapperAbility : PlayerAbility
{
    public string firePrefabName;
    public Transform raySpawn;
    public float maxRayDistance;
    public float instanceRate = 0.1f;
    public float duration = 3f;
    public LayerMask hitLayers;

    private Coroutine castFire;


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
                GameObject fire = PhotonNetwork.Instantiate(firePrefabName, hit.point, Quaternion.identity, 0, null);
                fire.transform.forward = new Vector3(raySpawn.forward.x, 0, raySpawn.forward.z);
            }
            yield return new WaitForEndOfFrame();
        }
        
    }

    IEnumerator StopFire()
    {
        yield return new WaitForSeconds(duration);
        StopCoroutine(castFire);
    }

    public override void Use()
    {
        base.Use();
        if(!inCooldown)
        {
            castFire = StartCoroutine(CastFire(instanceRate));
            StartCoroutine(StopFire());
            StartCoroutine(Cooldown());
        }
    }

}
