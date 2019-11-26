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
        naveManager = GetComponentInParent<NaveManager>();
        shield.SetActive(inShield);
        tipoCombustible = TipoCombustible.Escudo;
        GetFuel();
        animator = GetComponent<NaveAnimationManager>().animator;
    }

    public override void Use()
    {
        print("Entra al Use");
        //Activar el escudo siempre y cuando no haya un escudo activo
        if (inShield) return;
        

        //activar animacion escudo
        //GetComponentInParent<Animator>().SetBool("inShield",true);

        //activar sonido escudo
        //GetComponentInParent<AudioSource>().Play();
        

        if (combustible.currentAmmount < combustible.activeConsumption) return;

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

        StartCoroutine(DeactivateFuelAnimation("Turbo"));
        //desactivar escudo
        shield.SetActive(false);
        //GetComponentInParent<Animator>().SetBool("inShield",false);

        yield return new WaitForSeconds(cooldown);
        //desactivar variables de control de estado escudo
        inShield = false;
    }
}
