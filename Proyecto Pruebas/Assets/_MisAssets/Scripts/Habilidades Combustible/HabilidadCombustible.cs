﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class HabilidadCombustible : Photon.PunBehaviour
{
    //public Color color;
    public Combustible combustible;
    public TipoCombustible tipoCombustible;
    public NaveManager naveManager;
    public Animator animator;
    protected InputManager inputmanager;
    protected EffectManager effectManager;
    public bool inCooldown = false;
    public float currentCooldown;
    [Tooltip("Pon el cooldown de la habilidad, cuenta a partir de cuando se acaba")]
    public float cooldown;
    public float maxCooldown;


    public virtual void Use()
    {
        if (!effectManager)
        {
            effectManager = GetComponent<EffectManager>();
        }
        if (effectManager.SilenceFuels) return;
    }
    public void GetFuel()
    {
        Component[] combustibles;
        combustibles = GetComponents(typeof(Combustible));
        if (combustibles != null)
        {
            foreach (Combustible c in combustibles)
                if (c.tipoCombustible == tipoCombustible)
                {
                    combustible = c;
                }
        }
        inputmanager = GetComponent<InputManager>();
    }

    public IEnumerator ActivateFuelAnimation(string layerName)
    {
        for(int i=0;i<=10;i++)
        {
            animator.SetLayerWeight(animator.GetLayerIndex(layerName), Mathf.Lerp(0, 1, i * 0.1f));
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator DeactivateFuelAnimation(string layerName)
    {
        for (int i = 0; i <= 10; i++)
        {
            animator.SetLayerWeight(animator.GetLayerIndex(layerName), Mathf.Lerp(1, 0, i * 0.1f));
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator Cooldown(float _cooldown)
    {
        maxCooldown = _cooldown;
        currentCooldown = maxCooldown;
        inCooldown = true;
        while(currentCooldown>0)
        {
            currentCooldown -= Time.deltaTime;
            yield return null;
        }
        inCooldown = false;

    }

}
