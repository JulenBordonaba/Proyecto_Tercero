using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerShipAbility : ShipAbility
{

    public GameObject botsPrefab;

    private GameObject currentBots;

    public override void Use()
    {
        base.Use();
        if(!inCooldown)
        {
            Destroy(currentBots);
            currentBots = PhotonNetwork.Instantiate("SummonerBotsPrefab", GetComponent<NaveController>().modelTransform.position,Quaternion.identity,0,null);
            currentBots.transform.parent = GetComponent<NaveController>().modelTransform;
            currentBots.transform.localPosition = Vector3.zero;
            inCooldown = true;
            StartCoroutine(Cooldown());
        }



    }
}
