﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reparar : HabilidadCombustible
{
    [Tooltip("Pon la cantidad de vida que repara la nave por segundo")]
    public float repairAmmount;
    [Tooltip("Pon el cooldown de la habilidad, cuenta a partir de cuando se acaba")]
    public float cooldown;
    [Tooltip("Pon las partíaculas de la reparación")]
    public GameObject healingParticles;

    private bool isRepairing = false;
    private bool canRepair = true;
    private List<Pieza> piezas;

    private void Start()
    {
        piezas= new List<Pieza>( GetComponentsInChildren<Pieza>());
        healingParticles.SetActive(isRepairing);
        tipoCombustible = TipoCombustible.Reparar;
        GetFuel();
    }

    private void Update()
    {
        Repair();
    }

    public override void Use()
    {
        print("entra a Use");
        if (!canRepair) return;

        Combustible combustibleReparar = null; //variable para guardar el componente combustible de reparacion del objeto padre

        //activar animacion reparacion
        //GetComponentInParent<Animator>().SetBool("repair",true);

        //activar sonido reparqacion
        //GetComponentInParent<AudioSource>().Play();
        

        if (combustibleReparar == null) return;

        if (combustibleReparar.currentAmmount < combustibleReparar.activeConsumption) return;

        combustibleReparar.currentAmmount -= combustibleReparar.activeConsumption;

        isRepairing = true;
        canRepair = false;
        healingParticles.SetActive(true);
        NaveManager.combustible = combustibleReparar;
        StartCoroutine(Cooldown(combustibleReparar.duration));

    }

    private void Repair()
    {
        if (!isRepairing) return;
        foreach(Pieza pieza in piezas)
        {
            if(!pieza.isBroken)
            {
                pieza.currentHealth += repairAmmount * Time.deltaTime;
                pieza.CheckState();
            }
        }


    }

    private IEnumerator Cooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //desactivar variables de control de estado escudo
        isRepairing = false;
        healingParticles.SetActive(false);
        //GetComponentInParent<Animator>().SetBool("inShield",false);
        yield return new WaitForSeconds(cooldown);
        canRepair = true;
    }
}
