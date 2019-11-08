using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerAbility : PlayerAbility
{
    [Tooltip("Pon el prefab de la chatarra")]
    public GameObject chatarraPrefab;
    [Tooltip("POn la distancia a la que aparece la chatarra de la nave")]
    public float distance;

    private Transform modelTransform;

    private void Start()
    {
        modelTransform = GetComponent<NaveController>().modelTransform;
    }

    public override void Use()
    {
        base.Use();
        if (inCooldown) return;
        inCooldown = true;
        StartCoroutine(Cooldown());
        Instantiate(chatarraPrefab, modelTransform.position - modelTransform.forward * distance, Quaternion.identity);

    }
}
