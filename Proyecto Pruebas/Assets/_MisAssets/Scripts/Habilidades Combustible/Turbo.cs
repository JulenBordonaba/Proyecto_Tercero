using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbo : HabilidadCombustible
{
    private bool inJump = false;

    public override void Use()
    {
        //Activar el Turbo siempre y cuando no este en pleno Turbo       

        Combustible combustibleTurbo = new Combustible(); //variable para guardar el componente combustible del escudo del objeto padre        

        //if (GetComponentInParent<NaveManager>().Turbo == 0)

        //poner a true variable Salto en escudo
        //GetComponentInParent<NaveManager>().Turbo = 1;      // Propongo poner Turbo en la foncion de velocidad de forma: + (Turbo * StatTruboDeLaNave)

        //codigo que busca entre todos los combustibles del objeto y guarda el combustible del escudo. 
        //Así se pueden acceder a las variables del combustible del escudo
        Component[] combustibles;
        combustibles = GetComponentsInParent(typeof(Combustible));
        if (combustibles != null)
        {
            foreach (Combustible combustible in combustibles)
                if (combustible.tipoCombustible == TipoCombustible.Turbo)
                {
                    combustibleTurbo = combustible;
                }
        }
        else
        {
            //lo que sea
        }

        
    }
}
