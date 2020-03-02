using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapperFire : MonoBehaviour
{
    public float destroyTime = 3f;
    public float fireLoopTime = 0.3f;
    public float duration = 5f;
    public float fireDamage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PhotonView>())
        {
            print("Tiene photonview");
            if (other.gameObject.GetComponentInParent<PhotonView>().owner.NickName != GetComponentInParent<PhotonView>().owner.NickName)
            {
                print("es otro nickname");
                if (other.gameObject.GetComponentInParent<NaveManager>())
                {
                    print("hace daño");
                    other.gameObject.GetComponentInParent<NaveManager>().FireContact(fireDamage, fireLoopTime, duration);
                }

                //instanciar partículas explosión
                //Instantiate(explosionPrefab);
                //sonido
            }
        }
    }

}
