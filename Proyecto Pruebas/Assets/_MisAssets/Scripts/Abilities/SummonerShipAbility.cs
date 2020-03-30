using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerShipAbility : ShipAbility
{

    public GameObject botsPrefab;

    private GameObject currentBots;

    public override void Use(bool _forced)
    {
        base.Use(_forced);
        if(!inCooldown)
        {
            if(currentBots)
            {
                PhotonNetwork.Destroy(currentBots);
            }
            currentBots = PhotonNetwork.Instantiate("SummonerBotsPrefab", GetComponent<NaveController>().modelTransform.position,Quaternion.identity,0,null);
            currentBots.GetComponent<PhotonView>().RPC("SetParent", PhotonTargets.AllBuffered, photonView.owner.NickName);
            inCooldown = true;
            StartCoroutine(Cooldown(cooldown * (_forced ? 1.5f : 1f)));
        }



    }
    

}
