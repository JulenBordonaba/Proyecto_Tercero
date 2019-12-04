using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerAbility : PlayerAbility
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void Use()
    {
        base.Use();

        if(!inCooldown)
        {


            inCooldown = true;
            StartCoroutine(Cooldown());
        }


    }

}
