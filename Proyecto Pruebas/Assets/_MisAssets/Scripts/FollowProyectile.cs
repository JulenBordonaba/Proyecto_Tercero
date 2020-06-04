﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowProyectile : Photon.PunBehaviour
{

    [Tooltip("Pon la velocidad a la que saldrá el dron disparado")]
    public float velocity;
    [Tooltip("Pon la corrección de la trayectoria del bot")]
    public float trayectoryCorrection = 1;
    [Tooltip("Pon el aumento de velocidad del bot respecto al tiempo")]
    public float velocityIncrease = 10;
    public float damage = 100f;
    public AudioSource collisionAudioSource;


    private Vector3 direction;
    private Transform target;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseVelocity();
        Move();

        if(target)
        {
            Redirect();
        }
    }

    [PunRPC]
    public void SetDirection(Vector3 _direction)
    {
        direction = _direction.normalized;
    }

    [PunRPC]
    public void SetVelocity(float _velocity)
    {
        velocity = _velocity;
    }

    [PunRPC]
    public void SetTarget(string _nickname)
    {
        if(_nickname=="")
        {
            target = null;
            return;
        }
        foreach(NaveManager nm in GameManager.navesList)
        {
            if(nm.photonView.owner.NickName == _nickname)
            {
                target = nm.transform;
                return;
            }
        }
    }

    private void Redirect()
    {
        direction = Vector3.Lerp(direction, (target.position - transform.position).normalized, TargetDistance<20? 1 : Time.deltaTime * trayectoryCorrection).normalized;
        Debug.DrawRay(transform.position, direction * 1000, Color.blue);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + (direction * velocity * Time.deltaTime));
    }

    float TargetDistance
    {
        get { return Vector3.Distance(transform.position, target.position); }
    }

    private void IncreaseVelocity()
    {
        velocity += velocityIncrease * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.CompareTag("Floor") || other.CompareTag("Nave") || other.CompareTag("Obstacle"))) return;
        if (other.gameObject.GetComponentInParent<PhotonView>())
        {

            if (other.gameObject.GetComponentInParent<PhotonView>().owner.NickName != photonView.owner.NickName)
            {
                if (other.gameObject.GetComponentInParent<DamageManager>())
                {
                    PhotonView pv = other.gameObject.GetComponentInParent<DamageManager>().GetComponent<PhotonView>();
                    pv.RPC("TakeDamage", PhotonTargets.AllBuffered, damage, true);
                }

                //instanciar partículas explosión
                //Instantiate(explosionPrefab);
                //sonido
                PlayDestroySound();
                Destroy(gameObject);
            }
        }
        else
        {
            PlayDestroySound();
            Destroy(gameObject);
        }

        
    }

    void PlayDestroySound()
    {
        collisionAudioSource.transform.SetParent(null);
        collisionAudioSource.Play();
        Destroy(collisionAudioSource.gameObject,collisionAudioSource.clip.length);
    }
}
