using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reparar : HabilidadCombustible
{
    private bool repairing = false;

    public override void Use()
    {
        if (repairing == false) { 

        Combustible combustibleReparar = null; //variable para guardar el componente combustible de reparacion del objeto padre

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
                return;
            }

            repairing = true;
            StartCoroutine(Repairs(combustibleReparar.duration));
        }
    }

    private IEnumerator Repairs(float waitTime)
    {
        /*
         * piezas = GetComponentsInParent(typeof(Pieza));
         * foreach(Pieza pieza in piezas)                   // Se reparara cada pieza en funcion de la vida resante que le quede. una pieza al 10% se regenerara hasta un 33% por ejemplo
         *                                                  // Mientras que una al 85% subira hasta el 90%
         * {pieza.currenHealth += 100 - pieza.currenHealth * (Estat de reparar que la nave tenga) * Time.deltaTime }
         * 
         * 
         */

        yield return new WaitForSeconds(waitTime);

        //desactivar variables de control de estado escudo
        repairing = false;
        //GetComponentInParent<Animator>().SetBool("inShield",false);
    }
}
