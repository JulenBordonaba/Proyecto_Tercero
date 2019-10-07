using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PruebaNet : Photon.PunBehaviour
{
    public Transform spawn;

    public GameObject nave;

    private void Start()
    {
        InstantiateNave();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            InstantiateNave();
        }
    }

    public void InstantiateNave()
    {
        if (nave!=null) Destroy(nave);
        nave = PhotonNetwork.Instantiate("NaveAlex 1", spawn.position, Quaternion.identity, 0, null);
    }
}
