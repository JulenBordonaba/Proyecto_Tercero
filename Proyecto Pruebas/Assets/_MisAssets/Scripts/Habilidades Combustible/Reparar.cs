using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reparar : HabilidadCombustible
{
    private bool repairing = false;

    public override void Use()
    {
        if (repairing == false) { 

        Combustible combustibleReparar = new Combustible(); //variable para guardar el componente combustible de reparacion del objeto padre

        //activar animacion reparacion
        //GetComponentInParent<Animator>().SetBool("repair",true);

        //activar sonido reparqacion
        //GetComponentInParent<AudioSource>().Play();

        //codigo que busca entre todos los combustibles del objeto y guarda el combustible de reparar.  
        //Así se pueden acceder a las variables del combustible de reparar
            Component[] combustibles;
            combustibles = GetComponentsInParent(typeof(Combustible));
            if (combustibles != null)
            {
                foreach (Combustible combustible in combustibles)
                    if (combustible.tipoCombustible == TipoCombustible.Reparar)
                    {
                        combustibleReparar = combustible;
                    }
            }
            else
            {
                //lo que sea
            }

            repairing = true;
            StartCoroutine(Repairs(combustibleReparar.duration));
        }
    }

    private IEnumerator Repairs(float waitTime)
    {
        /*
         * 
         * 
         * 
         * 
         */

        yield return new WaitForSeconds(waitTime);

        //desactivar variables de control de estado escudo
        repairing = false;
        //GetComponentInParent<Animator>().SetBool("inShield",false);
    }
}
