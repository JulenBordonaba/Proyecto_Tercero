using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : Photon.PunBehaviour
{
    [Tooltip("Pon el cooldown de la habilidad")]
    public float cooldown;

    protected bool inCooldown = false;  //variable que controla cuando esta la habilidad en cooldown

    protected EffectManager effectManager;

    //Función que usa la habilidad
    public virtual void Use(float forcedCooldown)
    {
        if(!effectManager)
        {
            effectManager = GetComponent<EffectManager>();
        }
        if (effectManager.SilenceAbilities) return;
    }

    public IEnumerator Cooldown(float _cooldown)
    {
        yield return new WaitForSeconds(_cooldown);
        inCooldown = false;
    }

}
