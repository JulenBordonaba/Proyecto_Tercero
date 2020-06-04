using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerBotCollision : MonoBehaviour
{
    [Tooltip("Pon el daño que causa el bot al colisionar")]
    public float damage;
    public bool inShot = false;
    public LayerMask ignoreLayers;

    public AudioSource collisionAudioSource;

    private int shipLayer;

    private void Start()
    {
        StartCoroutine(SetLayer());
    }

    private void Update()
    {
        //print(GetComponentInParent<PhotonView>().owner.NickName);
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
                    PhotonView pv = other.gameObject.GetComponentInParent<DamageManager>().GetComponent<PhotonView>();
                    pv.RPC("TakeDamage",PhotonTargets.AllBuffered,damage, true);
                }

                //instanciar partículas explosión
                //Instantiate(explosionPrefab);
                //sonido
            }
        }
        PlayDestroySound();
        Destroy(transform.parent.gameObject);
    }

    IEnumerator SetLayer()
    {
        yield return new WaitForEndOfFrame();
        shipLayer = GetComponentInParent<NaveManager>().gameObject.layer;
    }

    void PlayDestroySound()
    {
        collisionAudioSource.transform.SetParent(null);
        collisionAudioSource.Play();
        Destroy(collisionAudioSource.gameObject, collisionAudioSource.clip.length);
    }
}
