using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerAbility : PlayerAbility
{
    public float effectRadius = 100f;

    public EffectData hackEffect;
    

    public override void Use(bool _forced)
    {
        base.Use(_forced);

        if (inCooldown) return;

        StartCoroutine(Cooldown(cooldown * (_forced ? 1.5f : 1f)));
    }

    public void Hack()
    {
        foreach(NaveManager nm in GameManager.navesList)
        {
            if (Vector3.Distance(transform.position, nm.transform.position) < effectRadius)
            {
                nm.photonView.RPC("StartEffect", PhotonTargets.All, hackEffect);
                nm.photonView.RPC("ForceUse", PhotonTargets.All, nm.photonView.owner.NickName);
            }
        }
    }
}
