﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerAbility : PlayerAbility
{
    public float dronVelocity = 200;
    public Transform dronSpawn;
    public Camera myCam;


    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


    public override void Use()
    {
        base.Use();

        if(!inCooldown)
        {
            GameObject dron = PhotonNetwork.Instantiate("BlackHoleDron", dronSpawn.position,Quaternion.identity,0,null);
            dron.GetComponent<BlackHoleDron>().Move(myCam.transform.forward, rb.velocity.magnitude+dronVelocity);

            inCooldown = true;
            StartCoroutine(Cooldown());
        }


    }

}
