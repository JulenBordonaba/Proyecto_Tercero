using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbo : HabilidadCombustible
{
    [Tooltip("pon el impulso que se le aplica a la nave al usar el turbo")]
    public float impulse;   //variable que contiene la fuerza del turbo
    [Tooltip("pon el tiempo que pasa despues de que acabe el turbo para poder volver  usarlo")]
    public float cooldown;  //tiempo que pasa despues de que acabe el turbo para poder volver  usarlo

    private bool inTurbo = false;

    private NaveController naveController;

    private void Start()
    {
        naveManager = GetComponentInParent<NaveManager>();
        naveController = GetComponent<NaveController>();

        tipoCombustible = TipoCombustible.Turbo;
        GetFuel();
        animator = GetComponent<NaveAnimationManager>().animator;
    }

    public override void Use()
    {
        //sale si ya esta enn turbo  
        if (inTurbo)
        {
            return;
        }      

        //if (GetComponentInParent<NaveManager>().Turbo == 0)

        //poner a true variable Salto en escudo
        //GetComponentInParent<NaveManager>().Turbo = 1;      // Propongo poner Turbo en la foncion de velocidad de forma: + (Turbo * StatTruboDeLaNave)

        

        if (combustible == null) return;

        if (combustible.currentAmmount < combustible.activeConsumption) return;

        combustible.currentAmmount -= combustible.activeConsumption;
        
        naveController.inBoost = true;
        inTurbo = true;
        StartCoroutine(Cooldown(combustible));
        StartCoroutine(ActivateFuelAnimation("Turbo"));
        naveManager.combustible = combustible;


    }

    

    private IEnumerator Cooldown(Combustible combustible)
    {
        yield return new WaitForSeconds(combustible.duration);
        naveController.inBoost = false;
        StartCoroutine(DeactivateFuelAnimation("Turbo"));
        yield return new WaitForSeconds(cooldown);
        inTurbo = false;
    }
}
