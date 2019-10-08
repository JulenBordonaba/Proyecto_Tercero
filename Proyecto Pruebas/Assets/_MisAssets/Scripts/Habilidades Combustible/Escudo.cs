using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : HabilidadCombustible
{
    

    public override void Use()
    {
        //Activar el escudo siempre y cuando no haya un escudo activo
        if (GetComponentInParent<NaveManager>().inShield == true) return;

        Combustible combustibleEscudo = new Combustible(); //variable para guardar el componente combustible del escudo del objeto padre

        //activar animacion escudo
        //GetComponentInParent<Animator>().SetBool("inShield",true);

        //activar sonido escudo
        //GetComponentInParent<AudioSource>().Play();

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
    }
    private IEnumerator DeactivateShield(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //desactivar variables de control de estado escudo
        GetComponentInParent<NaveManager>().inShield = false;
        //GetComponentInParent<Animator>().SetBool("inShield",false);
    }
}
