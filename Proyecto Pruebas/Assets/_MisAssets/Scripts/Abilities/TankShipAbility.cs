using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectManager))]
public class TankShipAbility : ShipAbility
{
    public EffectData damageReduction;

    private void Start()
    {

        effectManager = GetComponent<EffectManager>();
    }

    public override void Use(float forcedCooldown)
    {
        base.Use(forcedCooldown);
        if (effectManager.SilenceAbilities) return;

        if (!inCooldown)
        {
            inCooldown = true;
            audiSource.Play();
            StartCoroutine(Cooldown(cooldown * forcedCooldown));
            photonView.RPC("StartEffect", PhotonTargets.All, damageReduction.id);
        }

    }
    
}
