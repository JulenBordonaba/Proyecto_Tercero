using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleDron : Photon.PunBehaviour
{
    public float explosionRadius = 20;

    private Rigidbody rb;

    private List<GameObject> objectsInArea = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<SphereCollider>().radius = explosionRadius;
        Global.myShipType = "Scavenger";
    }

    // Update is called once per frame
    void Update()
    {

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
        
        foreach (GameObject go in objectsInArea)
        {
            if (go.GetComponent<Rigidbody>())
            {
                go.GetComponent<Rigidbody>().velocity = Vector3.zero;
                StartCoroutine(ExplosionAtraction(go));
            }
        }

        Destroy(gameObject,2);
    }

    IEnumerator ExplosionAtraction(GameObject go)
    {
        if (!go) yield break;


        if (go.GetComponentInParent<PhotonView>().owner.NickName != GetComponentInParent<PhotonView>().owner.NickName)
        {
            Vector3 pos = go.transform.position;
            for (int i = 1; i <= 5; i++)
            {
                go.transform.position = Vector3.Lerp(pos, transform.position, i * 0.2f);
                yield return new WaitForEndOfFrame();
            }
        }
          
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        print("objects in area " + objectsInArea.Count);
        photonView.RPC("Explosion", PhotonTargets.AllViaServer);
    }

    private void OnTriggerEnter(Collider other)
    {
        objectsInArea.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInArea.Remove(other.gameObject);
    }

}
