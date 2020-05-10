﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reparar : HabilidadCombustible
{
    [Tooltip("Pon la cantidad de vida que repara la nave por segundo")]
    public float repairAmmount;
    [Tooltip("Pon las partíaculas de la reparación")]
    public GameObject healingParticles;
    public bool isRepairing = false;

    private bool canRepair = true;
    private List<Pieza> piezas;

    private void Start()
    {
        naveManager = GetComponentInParent<NaveManager>();
        piezas = new List<Pieza>(GetComponentsInChildren<Pieza>());
        healingParticles.SetActive(isRepairing);
        tipoCombustible = TipoCombustible.Reparar;
        GetFuel();
        animator = GetComponent<NaveAnimationManager>().animator;
        effectManager = GetComponent<EffectManager>();
    }

    private void Update()
    {
        Repair();
        ActivateParticles();
        if (!photonView.isMine) return;
        if(inputmanager.UseRepair())
        {
            Use();
        }
    }

    public override void Use()
    {
        base.Use();
        if (effectManager.SilenceFuels) return;


        print("entra a Use");
        if (!canRepair) return;

        if (combustible == null) return;

        if (combustible.currentAmmount < combustible.activeConsumption) return;

        if (inCooldown) return;
        StartCoroutine(Cooldown(cooldown));

        combustible.currentAmmount -= combustible.activeConsumption;

        isRepairing = true;
        canRepair = false;
        healingParticles.SetActive(true);
        naveManager.combustible = combustible;
        StartCoroutine(ActivateFuelAnimation("Reparar"));
        StartCoroutine(myCooldown(combustible.duration));







    }

    public void ActivateParticles()
    {

        if (!healingParticles.activeInHierarchy && isRepairing)
        {
            healingParticles.SetActive(true);
        }
        else if(!isRepairing)
        {
            healingParticles.SetActive(false);
        }
    }
    

    private void Repair()
    {
        if (!isRepairing) return;
        foreach (Pieza pieza in piezas)
        {
            if (!pieza.isBroken)
            {
                pieza.Heal(repairAmmount * Time.deltaTime);
                pieza.CheckState();
            }
        }


    }

    private IEnumerator myCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //desactivar variables de control de estado escudo
        isRepairing = false;
        healingParticles.SetActive(false);
        StartCoroutine(DeactivateFuelAnimation("Reparar"));
        //GetComponentInParent<Animator>().SetBool("inShield",false);
        yield return new WaitForSeconds(cooldown);
        canRepair = true;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(isRepairing);
        }
        else
        {
            isRepairing = (bool)stream.ReceiveNext();
        }
    }
}
