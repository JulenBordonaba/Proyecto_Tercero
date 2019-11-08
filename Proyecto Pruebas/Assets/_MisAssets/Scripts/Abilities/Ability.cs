using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [Tooltip("Pon el cooldown de la habilidad")]
    public float cooldown;

    protected bool inCooldown = false;  //variable que controla cuando esta la habilidad en cooldown

    //Función que usa la habilidad
    public virtual void Use()
    {
        
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        inCooldown = false;
    }

}
