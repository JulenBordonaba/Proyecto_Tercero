using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShipAbility : ShipAbility
{

    public float damageReduction = 80f;
    public float duration = 6f;

    public bool isActive = false;

    public override void Use()
    {
        base.Use();

        if(!inCooldown)
        {
            StartCoroutine(Cooldown());

        }

    }

    public IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(duration);

    }
}
