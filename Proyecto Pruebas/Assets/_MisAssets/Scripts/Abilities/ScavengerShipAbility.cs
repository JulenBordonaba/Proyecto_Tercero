using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerShipAbility : ShipAbility
{
    
    [Tooltip("Pon la duración del imán")]
    public float duration;
    public ScavengerMagnet magnet;


    public override void Use()
    {
        base.Use();
        photonView.RPC("UseMagnet", PhotonTargets.All);
        

    }


    [PunRPC]
    public void UseMagnet()
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
            magnet.inverted = false;
            StartCoroutine(EndAbility());
        }
    }

    private IEnumerator EndAbility()
    {
        yield return new WaitForSeconds(duration);
        magnet.inUse = false;
        StartCoroutine(Cooldown());
    }
}
