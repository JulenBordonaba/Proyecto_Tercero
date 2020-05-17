using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleDron : Photon.PunBehaviour
{
    public float explosionRadius = 20;

    public int explosionFrames = 10;

    public float fallForce=50f;

    private Rigidbody rb;

    //private List<GameObject> objectsInArea = new List<GameObject>();

    private bool inExplosion = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //GetComponent<SphereCollider>().radius = explosionRadius/transform.localScale.x;
        foreach(Transform child in GetComponentsInChildren<Transform>())
        {
            child.SetGlobalScale(new Vector3(explosionRadius, explosionRadius, explosionRadius));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if(!inExplosion)
        {
            rb.AddForce(-Vector3.up * fallForce, ForceMode.Acceleration);
        }
    }


    [PunRPC]
    public void Move(Vector3 direction, float velocity)
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * velocity;
    }

    [PunRPC]
    public void Explosion()
    {
        
        foreach (NaveManager nm in GameManager.navesList)
        {
            if(Vector3.Distance(nm.transform.position,transform.position)<explosionRadius)
            {
                if(nm.photonView.owner.NickName==PhotonNetwork.player.NickName)
                {
                    
                    StartCoroutine(ExplosionAtraction(nm.gameObject));
                }
                
            }
                
            
        }
        StartCoroutine(DestroyObjectOnEndOfAtraction());
        
    }

    IEnumerator DestroyObjectOnEndOfAtraction()
    {

        for (int i = 1; i <= explosionFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    IEnumerator ExplosionAtraction(GameObject go)
    {
        if (!go)
        {
            yield break;
        }


        if (go.GetComponentInParent<PhotonView>().owner.NickName != GetComponentInParent<PhotonView>().owner.NickName)
        {
            Vector3 pos = go.transform.position;
            go.GetComponent<Rigidbody>().velocity = Vector3.zero;
            for (int i = 1; i <= explosionFrames; i++)
            {
                go.transform.position = Vector3.Lerp(pos, transform.position, i * 1f/(float)explosionFrames);
                yield return new WaitForEndOfFrame();
            }
        }

        Destroy(gameObject);
          
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (inExplosion) return;
        inExplosion = true;
        rb.velocity = Vector3.zero;
        //print("objects in area " + objectsInArea.Count);
        photonView.RPC("Explosion", PhotonTargets.AllViaServer);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.CompareTag("NaveCentre"))
    //    {
    //        objectsInArea.Add(other.gameObject);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if(objectsInArea.Contains(other.gameObject))
    //    {
    //        objectsInArea.Remove(other.gameObject);
    //    }
    //}

}
