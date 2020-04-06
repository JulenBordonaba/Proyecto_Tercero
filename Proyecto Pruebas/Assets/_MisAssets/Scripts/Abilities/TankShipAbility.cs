using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectManager))]
public class TankShipAbility : ShipAbility
{
    public EffectData damageReduction;
    
    

    public override void Use(bool _forced)
    {
        base.Use(_forced);

        if(!inCooldown)
        {
            inCooldown = true;
            StartCoroutine(Cooldown(cooldown * (_forced ? 1.5f : 1f)));
            photonView.RPC("StartEffect", PhotonTargets.All, damageReduction.id);
        }

    }
    
}
