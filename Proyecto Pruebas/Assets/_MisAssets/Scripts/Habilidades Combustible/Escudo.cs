﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : HabilidadCombustible
{
    [Tooltip("Pon el gameObject del escudo")]
    public GameObject shield;

    private bool inShield = false;
    

    private void Start()
    {
        naveManager = GetComponentInParent<NaveManager>();
        shield.SetActive(inShield);
        tipoCombustible = TipoCombustible.Escudo;
        GetFuel();
        animator = GetComponent<NaveAnimationManager>().animator;
        effectManager = GetComponent<EffectManager>();
    }


    private void Update()
    {
        if (!photonView.isMine) return;
        if(inputmanager.UseShield())
        {
            Use();
        }
    }

    public override void Use()
    {
        base.Use();
        if (effectManager.SilenceFuels) return;
        print("Entra al Use");
        //Activar el escudo siempre y cuando no haya un escudo activo
        if (inShield) return;
        if (inCooldown) return;
        StartCoroutine(Cooldown(cooldown));
        

        //activar animacion escudo
        //GetComponentInParent<Animator>().SetBool("inShield",true);

        //activar sonido escudo
        //GetComponentInParent<AudioSource>().Play();
        

        if (combustible.currentAmmount < combustible.activeConsumption) return;
        photonView.RPC("ActivateShield", PhotonTargets.All);
        
    }

    [PunRPC]
    public void ActivateShield()
    {
        combustible.currentAmmount -= combustible.activeConsumption;

        //poner a true variable estado en escudo
        inShield = true;
        shield.SetActive(true);
        print("pone el escudo");

        naveManager.combustible = combustible;

        StartCoroutine(ActivateFuelAnimation("Escudo"));
        //Inicar corrutina con la duración del escudo
        StartCoroutine(DeactivateShield(combustible.duration));
    }

    private IEnumerator DeactivateShield(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        StartCoroutine(DeactivateFuelAnimation("Escudo"));
        //desactivar escudo
        shield.SetActive(false);
        //GetComponentInParent<Animator>().SetBool("inShield",false);

        yield return new WaitForSeconds(cooldown);
        //desactivar variables de control de estado escudo
        inShield = false;
    }
}
