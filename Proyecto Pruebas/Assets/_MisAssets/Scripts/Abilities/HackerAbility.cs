using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerAbility : PlayerAbility
{
    public float effectRadius = 100f;

    public EffectData hackEffect;

    private NaveManager naveManager;

    private void Start()
    {
        naveManager = GetComponent<NaveManager>();
        effectManager = GetComponent<EffectManager>();
    }


    public override void Use(float forcedCooldown)
    {
        base.Use(forcedCooldown);
        if (effectManager.SilenceAbilities) return;

        if (inCooldown) return;
        audiSource.Play();
        Hack();
        StartCoroutine(Cooldown(cooldown * forcedCooldown));
    }

    public void Hack()
    {
        foreach(NaveManager nm in GameManager.navesList)
        {
            if(nm!= naveManager)
            {
                if (Vector3.Distance(transform.position, nm.transform.position) < effectRadius)
                {
                    nm.GetComponent<PhotonView>().RPC("StartEffect", PhotonTargets.All, hackEffect.id);
                }
            }
            
        }
    }
}
