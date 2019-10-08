using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : HabilidadCombustible
{
    

    public override void Use()
    {
        print("Usa Escudo");
        //activar animacion escudo
        //activar sonido escudo
        //poner a true variable estado en escudo, esta variable es del gameManager (NaveManager) de la Nave
        //iniciar un IEnumerator con la duración del tiempo.
            //mientras esta en escudo
                //si recibe un impacto
                    //activar sonido al recibir daño con escudo
                    //no hacer daño, esto en realidad debería ir en la función recibir daño
        //al acabarse el tiempo del escudo ponemos a false la variable estado en escudo
    }
}
