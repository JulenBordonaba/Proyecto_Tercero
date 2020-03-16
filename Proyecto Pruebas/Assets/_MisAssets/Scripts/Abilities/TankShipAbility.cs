using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectManager))]
public class TankShipAbility : ShipAbility
{
    public EffectData damageReduction;

    public bool isActive = false;
    

    public override void Use()
    {
        base.Use();

        if(!inCooldown)
        {
            StartCoroutine(Cooldown());
            photonView.RPC("StartEffect", PhotonTargets.All, damageReduction);
        }

    }
    
}
