﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRay : ShootWeapon
{
    [Tooltip("Pon la distancia máxima a la que llegará el disparo")]
    public float shootDistance;
    [Tooltip("Pon las layers contra las que choca el raycast del disparo")]
    public LayerMask layers;
    [Tooltip("Pon el prefab de las partículas que aparecen donde impacta el disparo")]
    public GameObject impactPrefab;

    public override void CastShot()
    {
        //declarar cariables para el raycast
        Ray ray = new Ray();
        RaycastHit hit;
        //configurar ray
        ray.origin = shotSpawn.position;
        ray.direction = transform.forward;
        //lanzar raycast
        if(Physics.Raycast(ray, out hit, shootDistance,layers))
        {
            //poner partículas de impacto
            GameObject impactGO = Instantiate(impactPrefab, hit.point, Quaternion.identity);
            impactGO.transform.up = hit.normal;
            //destruir efecto
            Destroy(impactGO, impactGO.GetComponent<ParticleSystem>().main.duration);
            //hacer daño al objetivo
            DamageObjective(hit.transform.gameObject);
        }
    }

    private void DamageObjective(GameObject other)
    {
        if(other.GetComponent<DamageManager>())
        {
            other.GetComponent<DamageManager>().TakeDamage(GetComponentInParent<Stats>().currentShotDamage);
        }
    }
}
