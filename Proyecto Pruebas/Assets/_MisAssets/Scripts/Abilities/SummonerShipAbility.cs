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
            currentBots = Instantiate(botsPrefab, GetComponent<NaveController>().modelTransform);
            currentBots.transform.localPosition = Vector3.zero;
            inCooldown = true;
            StartCoroutine(Cooldown());
        }



    }
}
