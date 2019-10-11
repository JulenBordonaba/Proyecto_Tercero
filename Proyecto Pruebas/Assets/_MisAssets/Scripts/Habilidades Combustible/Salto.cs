using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salto : HabilidadCombustible
{
    private bool inJump = false;

    public override void Use()
    {
        //Activar el Salto siempre y cuando no este en pleno salto       

        Combustible combustibleSalto = new Combustible(); //variable para guardar el componente combustible del escudo del objeto padre        

        //poner a true variable Salto 
        //GetComponentInParent<NaveManager>().inJump= true;

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
            //lo que sea
        }

        //Saltar
        /*
         * if(inJump == false){
         * inJump = true;
         * (Ecuaciones/formulas que usaremos para saltar)
         *  un rb.addforce hacia arriba?
         *  
         *  //activar animacion Salto
         *  //GetComponentInParent<Animator>().SetBool("jump",true);
         *
         *  //activar sonido salto
         *  //GetComponentInParent<AudioSource>().Play();
         *  
         * }
         */
    }

    public void Land()
    {
        //Se llama a esta funcion cuando la nave acabe un salto/vuelva a su altura de manejo
        inJump = false;
        //El salto a acabado, se puede volver a saltar
    }

}
