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
    }


    public override void Use(bool _forced)
    {
        base.Use(_forced);

        if (inCooldown) return;
        Hack();
        StartCoroutine(Cooldown(cooldown * (_forced ? 1.5f : 1f)));
    }

    public void Hack()
    {
        foreach(NaveManager nm in GameManager.navesList)
        {
            if(nm!= naveManager)
            {
                if (Vector3.Distance(transform.position, nm.transform.position) < effectRadius)
                {
                    nm.GetComponent<PhotonView>().RPC("StartEffect", PhotonTargets.All, hackEffect);
                    nm.GetComponent<AbilityManager>().ForceUse();
                }
            }
            
        }
    }
}
