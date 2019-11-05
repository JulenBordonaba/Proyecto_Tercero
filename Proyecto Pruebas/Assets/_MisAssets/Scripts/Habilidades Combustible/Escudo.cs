using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : HabilidadCombustible
{
    [Tooltip("Pon el gameObject del escudo")]
    public GameObject shield;
    [Tooltip("Pon el cooldown de la habilidad, cuenta a partir de cuando se acaba")]
    public float cooldown;

    private bool inShield = false;

    private void Start()
    {
        shield.SetActive(inShield);
    }

    public override void Use()
    {
        //Activar el escudo siempre y cuando no haya un escudo activo
        if (inShield == true) return;

        Combustible combustibleEscudo = null; //variable para guardar el componente combustible del escudo del objeto padre

        //activar animacion escudo
        //GetComponentInParent<Animator>().SetBool("inShield",true);

        //activar sonido escudo
        //GetComponentInParent<AudioSource>().Play();

        

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
            return;
        }
        if (combustibleEscudo == null) return;

        if (combustibleEscudo.currentAmmount < combustibleEscudo.activeConsumption) return;

        combustibleEscudo.currentAmmount -= combustibleEscudo.activeConsumption;

        //poner a true variable estado en escudo
        inShield = true;
        shield.SetActive(true);


        //Inicar corrutina con la duración del escudo
        StartCoroutine(DeactivateShield(combustibleEscudo.duration));
    }
    private IEnumerator DeactivateShield(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //desactivar escudo
        shield.SetActive(false);
        //GetComponentInParent<Animator>().SetBool("inShield",false);
        yield return new WaitForSeconds(cooldown);
        //desactivar variables de control de estado escudo
        inShield = false;
    }
}
