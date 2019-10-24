using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salto : HabilidadCombustible
{
    [Tooltip("Pon la fuerza del salto")]
    public float jumpForce; //variable que controla cuánta fuerza se impulsa la nave hacia arriba para saltar

    public override void Use()
    {
        if (GetComponent<NaveManager>().isPlanning) return;
        Combustible combustibleSalto = new Combustible(); //variable para guardar el componente combustible del escudo del objeto padre        
        

        //codigo que busca entre todos los combustibles del objeto y guarda el combustible del escudo. 
        //Así se pueden acceder a las variables del combustible del escudo
        Component[] combustibles;
        combustibles = GetComponentsInParent(typeof(Combustible));
        if (combustibles != null)
        {
            foreach (Combustible combustible in combustibles)
                if (combustible.tipoCombustible == TipoCombustible.Salto)
                {
                    combustibleSalto = combustible;
                }
        }
        else
        {
            return;
        }

        //Saltar
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        combustibleSalto.currentAmmount -= combustibleSalto.activeConsumption;

        //activar animacion Salto
        //GetComponentInParent<Animator>().SetBool("jump",true);
        
        //activar sonido salto
        //GetComponentInParent<AudioSource>().Play();
          
         
         
    }
    

}
