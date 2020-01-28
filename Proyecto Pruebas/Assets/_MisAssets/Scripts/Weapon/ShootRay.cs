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
    [Tooltip("Pon la layer del otro escudo")]
    public LayerMask otherShield;
    [Tooltip("Pon el radio de la zona de autoaim, en medidas de pantalla")]
    public float autoaimRadius;

    public Camera myCamera;

    private NaveManager myNaveManager;

    private void Awake()
    {
        myNaveManager = GetComponentInParent<NaveManager>();

    }

    public override void CastShot()
    {
        if (PauseManager.inPause) return;



        //declarar cariables para el raycast
        Ray ray = Autoaim();
        
        RaycastHit[] hits;

        hits = Physics.RaycastAll(ray, shootDistance, layers);

        //lanzar raycast
        if (hits.Length > 0)
        {

            RaycastHit nearestHit = hits[0];
            foreach (RaycastHit hit in hits)
            {
                if (!hit.collider.gameObject.GetComponentInParent<PhotonView>() || (hit.collider.gameObject.GetComponentInParent<PhotonView>() && !hit.collider.gameObject.GetComponentInParent<PhotonView>().isMine))
                {
                    if ((nearestHit.point - ray.origin).magnitude > (hit.point - ray.origin).magnitude)
                    {
                        nearestHit = hit;
                    }
                }
            }


            //poner partículas de impacto
            GameObject impactGO = Instantiate(impactPrefab, nearestHit.point, Quaternion.identity);
            impactGO.transform.up = nearestHit.normal;
            impactGO.GetComponentInChildren<ParticleSystem>().Play();

            //destruir efecto
            Destroy(impactGO, impactGO.GetComponentInChildren<ParticleSystem>().main.duration);

            //hacer daño al objetivo
            if (photonView.isMine)
            {
                DamageObjective(nearestHit.collider.gameObject);
            }



        }
    }

    private Ray Autoaim()
    {
        Vector2 screenMiddle = new Vector2(Screen.width * 0.5f, Screen.height * ((myCamera.rect.height * 0.5f) + myCamera.rect.y));
        //print(screenMiddle.y);
        Vector3 shipInScreenPoint;
        Vector3 playerPosition;

        Ray ray = new Ray();
        foreach (NaveManager nm in GameManager.navesList)
        {
            if (nm != myNaveManager)
            {
                if(nm!=null)
                {
                    playerPosition = nm.shipCentre.transform.position;
                    shipInScreenPoint = myCamera.WorldToScreenPoint(nm.transform.position);
                    //print(shipInScreenPoint);
                    //print("Camera Centre: " + screenMiddle);
                    //print(CircleCollision(screenMiddle.x, screenMiddle.y, autoaimRadius, shipInScreenPoint.x, shipInScreenPoint.y));
                    if (CircleCollision(screenMiddle.x, screenMiddle.y, autoaimRadius, shipInScreenPoint.x, shipInScreenPoint.y))
                    {
                        ray = new Ray();
                        ray.origin = shotSpawn.position;
                        ray.direction = (playerPosition - shotSpawn.position).normalized;
                        return ray;
                    }
                }
                

            }
        }

        ray.origin = shotSpawn.position;
        ray.direction = transform.forward;
        return ray;

    }

    private bool CircleCollision(float x, float y, float r, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow((x2 - x), 2) - Mathf.Pow((y2 - y), 2)) < r;
    }

    private void DamageObjective(GameObject other)
    {
        if (PhotonNetwork.connected)
        {
            if (other.layer == otherShield) return;
            //print(other.gameObject.name);
            if (other.GetComponentInParent<DamageManager>())
            {
                if (other.GetComponentInParent<PhotonView>())
                {
                    print("Id mandada: " + other.GetComponentInParent<DamageManager>().gameObject.GetComponent<PhotonView>().ownerId);
                    other.GetComponentInParent<NaveManager>().gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, GetComponentInParent<Stats>().currentShotDamage, true, other.GetComponentInParent<DamageManager>().damagedObject.ToString(), other.GetComponentInParent<DamageManager>().gameObject.GetComponent<PhotonView>().owner.NickName);
                }
            }
        }
        else
        {
            /*if (other.layer == otherShield) return;
            print(other.gameObject.name);
            if (other.GetComponent<DamageManager>())
            {
                other.GetComponent<DamageManager>().TakeDamage(GetComponentInParent<Stats>().currentShotDamage, true, other.gameObject);
            }
            else if (other.GetComponentInParent<DamageManager>())
            {
                other.GetComponentInParent<DamageManager>().TakeDamage(GetComponentInParent<Stats>().currentShotDamage, true, other.gameObject);
            }*/
        }

    }
}
