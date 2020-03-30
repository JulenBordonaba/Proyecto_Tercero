using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapperHarpon : Photon.PunBehaviour
{
    public float maxPull = 100000f;
    public float pullTime = 3f;
    

    SpringJoint springJoint;

    private void Start()
    {
        springJoint = GetComponent<SpringJoint>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer==LayerMask.GetMask("Player1"))
        {
            
            GameObject nave = collision.gameObject.GetComponentInParent<NaveManager>().gameObject;
            if(nave.GetComponent<PhotonView>().owner.NickName != photonView.owner.NickName)
            {
                transform.SetParent(nave.GetComponent<NaveController>().modelTransform);
                FixedJoint joint = nave.AddComponent<FixedJoint>();
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                joint.connectedBody = nave.GetComponent<Rigidbody>();
                StartCoroutine(Pull());
                Destroy(joint, pullTime);
                Destroy(gameObject, pullTime);
                Destroy(GetComponent<BoxCollider>());
            }
            
        }
    }

    IEnumerator Pull()
    {
        for (int i = 0; i <=pullTime*10; i++)
        {
            springJoint.spring = Mathf.Lerp(0, maxPull, Mathf.Pow((float)i / (pullTime * 10),2));
            yield return new WaitForSeconds(0.1f);
        }
    }
    
}
