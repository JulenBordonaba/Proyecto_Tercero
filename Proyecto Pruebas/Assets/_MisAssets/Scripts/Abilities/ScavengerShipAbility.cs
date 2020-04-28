using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerShipAbility : ShipAbility
{
    
    [Tooltip("Pon la duración del imán")]
    public float duration;
    public ScavengerMagnet magnet;

    private void Start()
    {

        effectManager = GetComponent<EffectManager>();
    }


    public override void Use(float forcedCooldown)
    {
        base.Use(forcedCooldown);
        if (effectManager.SilenceAbilities) return;
        photonView.RPC("UseMagnet", PhotonTargets.All, forcedCooldown);
        

    }


    [PunRPC]
    public void UseMagnet(float forcedCooldown)
    {
        if (inCooldown)
        {
            bool aux = !magnet.inverted;
            magnet.inverted = aux;
        }
        else
        {
            inCooldown = true;
            magnet.inUse = true;
            magnet.inverted = true;
            StartCoroutine(EndAbility());
            StartCoroutine(Cooldown(cooldown * forcedCooldown));
        }
    }

    private IEnumerator EndAbility()
    {
        yield return new WaitForSeconds(duration);
        magnet.inUse = false;
    }
}
