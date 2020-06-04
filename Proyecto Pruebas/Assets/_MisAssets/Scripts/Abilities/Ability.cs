using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectManager))]
public class Ability : Photon.PunBehaviour
{
    [Tooltip("Pon el cooldown de la habilidad")]
    public float cooldown;

    public AudioSource audiSource;
    [HideInInspector]
    public float currentCooldown=0f;

    public bool inCooldown = false;  //variable que controla cuando esta la habilidad en cooldown

    public EffectManager effectManager;

    //Función que usa la habilidad
    public virtual void Use(float forcedCooldown)
    {
        
    }

    public IEnumerator Cooldown(float _cooldown)
    {
        currentCooldown = _cooldown;
        while(currentCooldown>0)
        {
            currentCooldown -= Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(_cooldown);
        inCooldown = false;
    }

}
