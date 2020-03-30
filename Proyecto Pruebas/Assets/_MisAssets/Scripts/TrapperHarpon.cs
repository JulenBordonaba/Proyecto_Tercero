using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapperHarpon : Photon.PunBehaviour
{
    public float maxPull = 100000f;
    public float pullTime = 3f;
    

    SpringJoint springJoint;

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
        springJoint = GetComponent<SpringJoint>();
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
