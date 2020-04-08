using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapperFire : MonoBehaviour
{
    public float destroyTime = 3f;
    public EffectData fireDamage;
    PhotonView myPhotonView;
    // Start is called before the first frame update
    void Start()
    {
        myPhotonView = GetComponentInParent<PhotonView>();
    }


    private void OnEnable()
    {
        StartCoroutine(DisableObject());
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(destroyTime);
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Radar")) return;
        //print("al entrar: " + other.gameObject.name);
        //print("entra al trigger");
        if (other.gameObject.GetComponentInParent<PhotonView>())
        {
            //print("Tiene photonview");
            if (other.gameObject.GetComponentInParent<EffectManager>())
            {
                //print("tiene effectManager");
                //print("cuando comprueba si tiene effect manager: " + other.gameObject.name);
                PhotonView pv = other.gameObject.GetComponentInParent<EffectManager>().GetComponent<PhotonView>();
                if (pv.owner.NickName !=myPhotonView.owner.NickName)
                {
                    //print("pone el efecto");
                    //print("---------------------------------------------al aplicar el efecto: " + other.gameObject.name);
                    pv.RPC("StartEffect", PhotonTargets.All, fireDamage.id);
                }

                //instanciar partículas explosión
                //Instantiate(explosionPrefab);
                //sonido
            }
        }
    }

}
