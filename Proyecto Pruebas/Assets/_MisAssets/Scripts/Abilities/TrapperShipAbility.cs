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
    }

    public override void Use(bool _forced)
    {
        base.Use(_forced);

        if(!inCooldown)
        {
            //instanciar arpón con photonview
            GameObject harpon=PhotonNetwork.Instantiate(harponPrefabName, harponPivot.position, harponPivot.rotation, 0);
            //asignar velocidad al rigidbody del arpón
            harpon.GetComponent<Rigidbody>().velocity = harpon.transform.forward * (rb.velocity.magnitude+harponVelocity);
            //asignar maxPull y Pulltime al arpón
            TrapperHarpon th = harpon.GetComponent<TrapperHarpon>();
            photonView.RPC("ConfigureTrapperHarpon", PhotonTargets.All, th, maxPull, pullTime);

            StartCoroutine(Cooldown(cooldown * (_forced ? 1.5f : 1f)));
        }
    }

    [PunRPC]
    public void ConfigureTrapperHarpon(TrapperHarpon _harpon, float _maxPull)
    {
        _harpon.maxPull = _maxPull;
    }
}
