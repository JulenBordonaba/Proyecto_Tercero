using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerShipAbility : ShipAbility
{
    public float duration = 5f;

    public GameObject[] holograms;
    public GameObject[] hologramPositions;

    private NaveController naveController;


    private void Start()
    {
        naveController = GetComponent<NaveController>();
    }

    public override void Use()
    {
        base.Use();
        if (!inCooldown)
        {
            StartCoroutine(Cooldown());
            StartCoroutine(HologramDuration());
            int position = Random.Range(0, hologramPositions.Length - holograms.Length + 1);
            photonView.RPC("SetHolograms", PhotonTargets.All, position);
        }

    }

    IEnumerator HologramDuration()
    {
        yield return new WaitForSeconds(duration);
        photonView.RPC("StopHolograms", PhotonTargets.All);
    }

    [PunRPC]
    public void StopHolograms()
    {
        foreach (GameObject go in holograms)
        {
            go.SetActive(false);
        }
    }


    [PunRPC]
    public void SetHolograms(int position)
    {
        for (int i = 0; i < holograms.Length; i++)
        {
            Ray ray = new Ray();
            ray.direction = -Vector3.up;
            ray.origin = hologramPositions[i + position].transform.position + Vector3.up * 30;
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
            {
                holograms[i].transform.parent = hologramPositions[i + position].transform;
                holograms[i].transform.localPosition = Vector3.up * 5;
                holograms[i].transform.SetParent(null);
                holograms[i].GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
                holograms[i].GetComponentInChildren<RadarTarget>().gameObject.SetActive(true);
                holograms[i].transform.rotation = naveController.transform.rotation;
                holograms[i].GetComponent<NaveController>().modelTransform.rotation = naveController.modelTransform.rotation;
                holograms[i].SetActive(true);
            }


                
        }

        GameManager.navesList.barajar<NaveManager>();
    }
}
