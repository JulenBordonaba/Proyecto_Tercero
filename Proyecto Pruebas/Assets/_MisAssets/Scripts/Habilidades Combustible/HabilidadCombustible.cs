using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HabilidadCombustible : MonoBehaviour
{
    //public Color color;
    public Combustible combustible;
    public abstract void Use();
    public TipoCombustible tipoCombustible;
    public NaveManager naveManager;
    public Animator animator;

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

}
