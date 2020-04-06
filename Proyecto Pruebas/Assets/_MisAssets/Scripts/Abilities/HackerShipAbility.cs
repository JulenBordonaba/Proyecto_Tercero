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



    public override void Use(bool _forced)
    {
        base.Use(_forced);
        if (!inCooldown)
        {
            inCooldown = true;
            StartCoroutine(Cooldown(cooldown*(_forced?1.5f:1f)));
            StartCoroutine(HologramDuration());
            List<int> positions = new List<int>();
            for (int i = 0; i < (holograms.Length<hologramPositions.Length? holograms.Length : hologramPositions.Length) ; i++)
            {
                int position = 0;
                do
                {
                    position = Random.Range(0,hologramPositions.Length);
                } while (positions.Contains(position));
                positions.Add(position);
            }
            //int position = Random.Range(0, hologramPositions.Length - holograms.Length + 1);
            photonView.RPC("SetHolograms", PhotonTargets.All, positions.ToArray());
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
            go.GetComponentInChildren<RadarTarget>().Disable();
            go.SetActive(false);
        }
    }


    [PunRPC]
    public void SetHolograms(int[] positions)
    {
        for (int i = 0; i < (holograms.Length < positions.Length ? holograms.Length : positions.Length); i++)
        {
            Ray ray = new Ray();
            ray.direction = -Vector3.up;
            ray.origin = hologramPositions[positions[i]].transform.position + Vector3.up * 30;
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
            {
                holograms[i].transform.parent = hologramPositions[positions[i]].transform;
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
