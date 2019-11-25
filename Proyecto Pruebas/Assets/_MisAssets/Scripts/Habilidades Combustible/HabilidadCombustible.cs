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

}
