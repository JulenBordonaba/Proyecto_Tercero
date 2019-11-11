using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerBotCollision : MonoBehaviour
{
    [Tooltip("Pon el daño que causa el bot al colisionar")]
    public float damage;
    public bool inShot = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!inShot) return;
        if(other.gameObject.layer!=gameObject.layer)
        {
            if(other.gameObject.GetComponentInParent<DamageManager>())
            {
                other.gameObject.GetComponentInParent<DamageManager>().TakeDamage(damage);
            }
            Destroy(transform.parent.gameObject);
            //instanciar partículas explosión
            //Instantiate(explosionPrefab);
            //sonido
        }
    }
}
