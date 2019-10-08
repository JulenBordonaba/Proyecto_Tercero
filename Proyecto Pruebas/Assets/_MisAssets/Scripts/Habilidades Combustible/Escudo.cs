using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : HabilidadCombustible
{
    

    public override void Use()
    {
        Combustible combustibleEscudo = new Combustible();
        //print("Usa Escudo");
        //activar animacion escudo
        //activar sonido escudo

        //poner a true variable estado en escudo
        GetComponentInParent<NaveManager>().inShield = true;


        //codigo que busca entre todos los combustibles del objeto y guarda el combustible del escudo. 
        //Así se pueden acceder a las variables del combustible del escudo
                Component[] combustibles;
                combustibles = GetComponentsInParent(typeof(Combustible));
                if (combustibles != null)
                {
                    foreach (Combustible combustible in combustibles)
                        if (combustible.tipoCombustible == TipoCombustible.Escudo)
                        {
                            combustibleEscudo = combustible;
                        }
                }
                else
                {
                    //lo que sea
                }

        //Inicar corrutina con la duración del escudo
        StartCoroutine(DeactivateShield(combustibleEscudo.duration));

            //mientras esta en escudo
                //si recibe un impacto
                    //activar sonido al recibir daño con escudo
                    //no hacer daño, esto en realidad debería ir en la función recibir daño
        //al acabarse el tiempo del escudo ponemos a false la variable estado en escudo
    }
    private IEnumerator DeactivateShield(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetComponentInParent<NaveManager>().inShield = false;
    }
}
