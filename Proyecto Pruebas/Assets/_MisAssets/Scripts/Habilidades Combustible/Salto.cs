using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salto : HabilidadCombustible
{
    [Tooltip("Pon la fuerza del salto")]
    public float jumpForce; //variable que controla cuánta fuerza se impulsa la nave hacia arriba para saltar
    [Tooltip("pon el cooldown del salto")]
    public float cooldown;

    private bool inJump = false;
    public override void Use()
    {
        if (GetComponent<NaveManager>().isPlanning || inJump) return;
        Combustible combustibleSalto=null; //variable para guardar el componente combustible del escudo del objeto padre        
        

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
        inJump = true;
        StartCoroutine(Cooldown());
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if(combustibleSalto!=null)
        {

            combustibleSalto.currentAmmount -= combustibleSalto.activeConsumption;
        }

        //activar animacion Salto
        //GetComponentInParent<Animator>().SetBool("jump",true);
        
        //activar sonido salto
        //GetComponentInParent<AudioSource>().Play();
          
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        inJump = false;
    }
    

}
