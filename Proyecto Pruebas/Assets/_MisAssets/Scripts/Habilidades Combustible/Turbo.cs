﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbo : HabilidadCombustible
{
    public float impulse;   //variable que contiene la fuerza del turbo

    private bool inTurbo = false;
    public override void Use()
    {
        //Activar el Turbo siempre y cuando no este en pleno Turbo       
        if (inTurbo) return;
        Combustible combustibleTurbo = null; //variable para guardar el componente combustible del escudo del objeto padre        

        //if (GetComponentInParent<NaveManager>().Turbo == 0)

        //poner a true variable Salto en escudo
        //GetComponentInParent<NaveManager>().Turbo = 1;      // Propongo poner Turbo en la foncion de velocidad de forma: + (Turbo * StatTruboDeLaNave)

        //codigo que busca entre todos los combustibles del objeto y guarda el combustible del escudo. 
        //Así se pueden acceder a las variables del combustible del escudo
        Component[] combustibles;
        combustibles = GetComponentsInParent(typeof(Combustible));
        if (combustibles != null)
        {
            foreach (Combustible combustible in combustibles)
                if (combustible.tipoCombustible == TipoCombustible.Turbo)
                {
                    combustibleTurbo = combustible;
                }
        }
        else
        {
            return;
        }


        GetComponent<Rigidbody>().AddForce(GetComponent<NaveController>().modelTransform.forward * impulse, ForceMode.Acceleration);
        GetComponent<NaveController>().inBoost = true;
        StartCoroutine(Cooldown(combustibleTurbo));
        
    }

    private IEnumerator Cooldown(Combustible combustible)
    {
        yield return new WaitForSeconds(combustible.duration);
        inTurbo = false;
        GetComponent<NaveController>().inBoost = false;
    }
}
