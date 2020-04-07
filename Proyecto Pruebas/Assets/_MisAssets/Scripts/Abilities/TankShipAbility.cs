using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectManager))]
public class TankShipAbility : ShipAbility
{
    public EffectData damageReduction;
    
    

    public override void Use(float forcedCooldown)
    {
        base.Use(forcedCooldown);

        if(!inCooldown)
        {
            inCooldown = true;
            StartCoroutine(Cooldown(cooldown * forcedCooldown));
            photonView.RPC("StartEffect", PhotonTargets.All, damageReduction.id);
        }

    }
    
}
