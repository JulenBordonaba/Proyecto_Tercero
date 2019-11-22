﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salto : HabilidadCombustible
{
    [Tooltip("Pon la fuerza del salto")]
    public float jumpForce; //variable que controla cuánta fuerza se impulsa la nave hacia arriba para saltar
    [Tooltip("pon el cooldown del salto")]
    public float cooldown;

    private bool inJump = false;


    private void Start()
    {

        tipoCombustible = TipoCombustible.Salto;
        GetFuel();
    }

    public override void Use()
    {
        if (GetComponent<NaveManager>().isPlanning || inJump) return;       

        
        if (combustible == null) return;

        if (combustible.currentAmmount < combustible.activeConsumption) return;

        combustible.currentAmmount -= combustible.activeConsumption;

        //Saltar
        inJump = true;
        StartCoroutine(Cooldown());
        GetComponent<Rigidbody>().AddForce(/*Vector3.up*/GetComponent<NaveController>().modelTransform.up * jumpForce, ForceMode.Impulse);
        NaveManager.combustible = combustible;

        //activar animacion Salto
        //GetComponentInParent<Animator>().SetBool("jump",true);

        //activar sonido salto
        //GetComponentInParent<AudioSource>().Play();

    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        inJump = false;
    }


}
