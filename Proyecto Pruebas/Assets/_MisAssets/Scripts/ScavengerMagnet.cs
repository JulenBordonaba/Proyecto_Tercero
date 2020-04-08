using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerMagnet : MonoBehaviour
{
    [Tooltip("Pon la fuerza del imán")]
    public float magnetForce;

    public bool inverted = false;
    public bool inUse = false;

    private NaveManager naveManager;
    public float effectRadius = 100f;

    private void Start()
    {
        naveManager = GetComponentInParent<NaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inUse)
        {
            DoEffect();
        }
    }

    void DoEffect()
    {
        foreach(NaveManager nm in GameManager.navesList)
        {
            if(nm.photonView.owner.NickName==PhotonNetwork.player.NickName && nm.photonView.owner.NickName!=naveManager.photonView.owner.NickName)
            {
                if(Vector3.Distance(naveManager.transform.position, nm.transform.position)<effectRadius)
                {
                    print("debería imantar la nave");
                    Vector3 direction = (naveManager.transform.position - nm.transform.position).normalized;
                    nm.rb.AddForce(direction * magnetForce * (inverted ? -1 : 1), ForceMode.VelocityChange);
                }
            }
        }
    }

   
}
