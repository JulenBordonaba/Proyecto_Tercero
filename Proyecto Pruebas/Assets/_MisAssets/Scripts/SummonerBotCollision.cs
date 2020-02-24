using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerBotCollision : MonoBehaviour
{
    [Tooltip("Pon el daño que causa el bot al colisionar")]
    public float damage;
    public bool inShot = false;
    public LayerMask ignoreLayers;

    private int shipLayer;

    private void Start()
    {
        shipLayer = GetComponentInParent<NaveManager>().gameObject.layer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!inShot) return;
        //if(((1 << other.gameObject.layer) & ignoreLayers) != 0)
        if(other.gameObject.GetComponentInParent<PhotonView>())
        {
            if (other.gameObject.GetComponentInParent<PhotonView>().owner.NickName != GetComponentInParent<PhotonView>().owner.NickName)
            {
                if (other.gameObject.GetComponentInParent<DamageManager>())
                {
                    other.gameObject.GetComponentInParent<DamageManager>().TakeDamage(damage, true);
                }

                //instanciar partículas explosión
                //Instantiate(explosionPrefab);
                //sonido
            }
        }
        
        Destroy(transform.parent.gameObject);
    }

    
}
