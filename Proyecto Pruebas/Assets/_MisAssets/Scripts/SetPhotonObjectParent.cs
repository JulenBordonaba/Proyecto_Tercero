using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
public class SetPhotonObjectParent : Photon.PunBehaviour
{
    [PunRPC]
    public void SetParent(string parentNickname)
    {
        foreach (NaveManager nm in GameManager.navesList)
        {
            if (nm.GetComponent<PhotonView>().owner.NickName == parentNickname)
            {
                print("recoloca el objeto");
                transform.SetParent(nm.GetComponent<NaveController>().modelTransform);
                transform.localPosition = Vector3.zero;
            }
        }
    }
}
