using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaken : MonoBehaviour
{
    [Tooltip("Segundos durante los cuales la nave se hace inmune tras recibir daño. Se recomienta algo muy pequeño como 0.5")]
    public float inmuneTimeDuration; 

    bool damageInmune = false; // variable de estado para saber si es inmune al daño o no
    bool inShield = false; // variable de estado para saber si tiene escudo o no
    
    
    private void OnCollisionEnter(Collision collision)
    {
        // si el objeto es una nave y tiene activo el escudo, no puede recibe daño
        //if (GetComponentInParent<NaveManager>() != null && GetComponentInParent<NaveManager>().inShield == true)
        {
            inShield = true;
        }
        //si el objeto es una nave y no tiene el escudo activo puede recibir daño
        //else if (GetComponentInParent<NaveManager>() != null && GetComponentInParent<NaveManager>().inShield == false)
        {
            inShield = false;
        }

        if (!inShield) // si no tiene el escudo activado revibe daño
        {
            if(collision.gameObject.tag!="shot") // si el daño se va a producir por un choque físico
            {
                if (!damageInmune) // si no es inmune se reproduce la animación de daño y los sonidos correspondientes
                {
                    GetComponentInParent<Stats>().currentLife = GetComponentInParent<Stats>().currentLife - collision.gameObject.GetComponent<Stats>().currentCollisionDamage;
                    // sonido chocar
                }

            }
            else // si el daño se produce por un disparo
            {
                if (!damageInmune) // si no es inmune se reproduce la animación de daño y los sonidos correspondientes
                {
                    GetComponentInParent<Stats>().currentLife = GetComponentInParent<Stats>().currentLife - collision.gameObject.GetComponent<Stats>().currentShotDamage;
                    // sonido recibir un disparo
                }
            }
            damageInmune = true;
            StartCoroutine(MakeVulnerableCoroutine(inmuneTimeDuration));
        }

        if (!damageInmune) // si no es inmune se reproduce la animación de daño y los sonidos correspondientes
        {

            // reproducir una animación y sonido sonido según tenga el escudo activo o no
            //if (inShield)
            //{
            //  // animación del escudo al recibir daño
            //  // sonido del escudo al recibir daño
            //}
            //else
            //{
            //  // animación al recibir daño
            //  // sonido de la pieza recibiendo daño
            //}
        }



    }

    // corutina para volver vulnerable a la nave después de esperar un poco al recibir un golpe
    IEnumerator MakeVulnerableCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        damageInmune = false;
    }
}
