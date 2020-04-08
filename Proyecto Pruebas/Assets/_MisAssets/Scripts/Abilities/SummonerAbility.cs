using System.Collections;
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
        effectManager = GetComponent<EffectManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


    public override void Use(float forcedCooldown)
    {
        base.Use(forcedCooldown);
        if (effectManager.SilenceAbilities) return;

        if (!inCooldown)
        {
            GameObject dron = PhotonNetwork.Instantiate("BlackHoleDron", dronSpawn.position,Quaternion.identity,0,null);
            dron.GetComponent<BlackHoleDron>().photonView.RPC("Move", PhotonTargets.All,myCam.transform.forward, rb.velocity.magnitude+dronVelocity);

            inCooldown = true;
            StartCoroutine(Cooldown(cooldown * forcedCooldown));
        }


    }

}
