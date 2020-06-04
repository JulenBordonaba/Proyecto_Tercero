using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapperShipAbility : ShipAbility
{
    public string harponPrefabName = "TrapperHarpon";
    public Transform harponPivot;
    public float harponVelocity=500f;

    public float maxPull = 100000f;
    public float pullTime = 3f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        effectManager = GetComponent<EffectManager>();
    }

    public override void Use(float forcedCooldown)
    {
        base.Use(forcedCooldown);
        if (effectManager.SilenceAbilities) return;

        if(!inCooldown)
        {
            inCooldown = true;
            audiSource.Play();
            //instanciar arpón con photonview
            GameObject harpon=PhotonNetwork.Instantiate(harponPrefabName, harponPivot.position, harponPivot.rotation, 0);
            //asignar velocidad al rigidbody del arpón
            harpon.GetComponent<Rigidbody>().velocity = harpon.transform.forward * (rb.velocity.magnitude+harponVelocity);
            //asignar maxPull y Pulltime al arpón
            TrapperHarpon th = harpon.GetComponent<TrapperHarpon>();
            th.photonView.RPC("SetForce", PhotonTargets.All, maxPull);

            StartCoroutine(Cooldown(cooldown * forcedCooldown));
        }
    }
    
}
