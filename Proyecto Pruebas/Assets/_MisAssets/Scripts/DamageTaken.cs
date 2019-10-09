using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaken : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag!="shot") // si el daño se va a producir por un choque físico
        {
            GetComponentInParent<Stats>().life = GetComponentInParent<Stats>().life - collision.gameObject.GetComponent<Stats>().collisionDamage;
        }
        else // si el daño se produce por un disparo
        {
            GetComponentInParent<Stats>().life = GetComponentInParent<Stats>().life - collision.gameObject.GetComponent<Stats>().shotDamage;
        }
         
    }
}
