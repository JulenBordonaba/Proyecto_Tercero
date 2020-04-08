using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerShipAbility : ShipAbility
{

    public GameObject botsPrefab;

    private GameObject currentBots;
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
            if(currentBots)
            {
                PhotonNetwork.Destroy(currentBots);
            }
            currentBots = PhotonNetwork.Instantiate("SummonerBotsPrefab", GetComponent<NaveController>().modelTransform.position,Quaternion.identity,0,null);
            currentBots.GetComponent<PhotonView>().RPC("SetParent", PhotonTargets.AllBuffered, photonView.owner.NickName);
            inCooldown = true;
            StartCoroutine(Cooldown(cooldown * forcedCooldown));
        }



    }
    

}
