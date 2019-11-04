using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPrefab : ShootWeapon
{

    [Tooltip("Pon el prefab del disparo")]
    public GameObject shotPrefab;
    [Tooltip("Pon la velocidad a la que se disparará")]
    public float shotForce = 100;
    
    public override void CastShot()
    {
        //instanciar disparo
        GameObject shot = Instantiate(shotPrefab, shotSpawn.position, transform.rotation);
        //dar impulso al disparo
        if (shot.GetComponent<Rigidbody>() != null)
        {
            shot.GetComponent<Rigidbody>().AddForce(transform.forward * shotForce, ForceMode.VelocityChange);
        }
        //destruir disparo al rato
        Destroy(shot, 5f);
    }
}
