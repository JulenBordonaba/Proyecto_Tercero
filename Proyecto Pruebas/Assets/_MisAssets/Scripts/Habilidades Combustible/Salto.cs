﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salto : HabilidadCombustible
{
    [Tooltip("Pon la fuerza del salto")]
    public float jumpForce; //variable que controla cuánta fuerza se impulsa la nave hacia arriba para saltar

    private bool inJump = false;


    private void Start()
    {
        naveManager = GetComponentInParent<NaveManager>();
        tipoCombustible = TipoCombustible.Salto;
        GetFuel();
        animator = GetComponent<NaveAnimationManager>().animator;
        effectManager = GetComponent<EffectManager>();
    }

    private void Update()
    {
        if (!photonView.isMine) return;
        if (inputmanager.UseJump())
        {
            Use();
        }
    }


    public override void Use()
    {
        base.Use();
        if (effectManager.SilenceFuels) return;
        if (GetComponent<NaveManager>().isPlanning || inJump) return;

        if (inCooldown) return;
        StartCoroutine(Cooldown(cooldown + combustible.duration));


        if (combustible == null) return;

        if (combustible.currentAmmount < combustible.activeConsumption) return;

        combustible.currentAmmount -= combustible.activeConsumption;

        //Saltar
        inJump = true;
        StartCoroutine(ActivateFuelAnimation("Salto"));
        StartCoroutine(Cooldown());
        GetComponent<Rigidbody>().AddForce(/*Vector3.up*/GetComponent<NaveController>().modelTransform.up * jumpForce, ForceMode.VelocityChange);
        naveManager.combustible = combustible;

        //activar animacion Salto
        //GetComponentInParent<Animator>().SetBool("jump",true);

        //activar sonido salto
        //GetComponentInParent<AudioSource>().Play();

    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(combustible.duration);

        StartCoroutine(DeactivateFuelAnimation("Salto"));
        yield return new WaitForSeconds(cooldown);
        inJump = false;
    }


}
