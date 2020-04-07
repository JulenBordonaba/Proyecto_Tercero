using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapperHarpon : Photon.PunBehaviour
{
    public float maxPull = 100000f;


    //SpringJoint springJoint;
    Transform parentTransform;
    Rigidbody rb;

    #region PhotonSerialize
    public byte classId { get; set; }

    public static object Deserialize(byte[] data)
    {
        TrapperHarpon result = new TrapperHarpon();
        result.classId = data[0];
        return result;
    }

    public static byte[] Serialize(object customType)
    {
        TrapperHarpon c = (TrapperHarpon)customType;
        return new byte[] { c.classId };
    }
    #endregion

    private void Start()
    {
        //springJoint = GetComponent<SpringJoint>();
        rb = GetComponent<Rigidbody>();
        GetParentShip();
    }

    void GetParentShip()
    {
        foreach(NaveManager nm in GameManager.navesList)
        {
            if(nm.GetComponent<PhotonView>().owner.NickName==photonView.owner.NickName)
            {
                parentTransform = nm.transform;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    [PunRPC]
    public void SetForce(float _maxPull)
    {
        maxPull = _maxPull;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collision");
        string otherLayer = LayerMask.LayerToName(collision.gameObject.layer);
        print(otherLayer);



        if (otherLayer=="Player1")
        {
            print("Layer Player1");
            GameObject nave = collision.gameObject.GetComponentInParent<NaveManager>().gameObject;
            if(nave.GetComponent<PhotonView>().owner.NickName != photonView.owner.NickName)
            {
                print("pull");

                //emparentar arpón con el objeto con el que ha colisionado
                transform.SetParent(collision.gameObject.transform);

                //configurar Rigidbody
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;

                //Calcular fuerza
                Vector3 forceDirection = parentTransform.position - transform.position;
                Vector3 force = (forceDirection.normalized * maxPull * ( Vector3.Distance(parentTransform.position, transform.position) /100f)) + (Vector3.up * maxPull*0.1f);

                //aplicar fuerza
                nave.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                
                Destroy(GetComponent<BoxCollider>());
            }
            
        }
        else
        {

            //emparentar arpón con el objeto con el que ha colisionado
            transform.SetParent(collision.gameObject.transform);

            //configurar Rigidbody
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }


    
    
}
