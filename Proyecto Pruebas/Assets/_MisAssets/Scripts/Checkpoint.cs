using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float fuelRechargeAmmount = 20f;
    public bool isFinal = false;
    public GameObject checkpointGO;
    public GameObject newestIcon;
    public GameObject notNewestIcon;

    private List<NaveManager> passedShips = new List<NaveManager>();

    public void Unlock()
    {
        CheckpointManager.newest = this;
        checkpointGO.SetActive(true);
        GetComponentInChildren<RadarTarget>().radarImage = newestIcon;
    }

    private bool CheckShips(NaveManager naveManager)
    {
        foreach(NaveManager nm in passedShips)
        {
            if(nm==naveManager)
            {
                return false;
            }
        }
        passedShips.Add(naveManager);
        return true;
    }

    private void RechargeFuel(NaveManager naveManager)
    {
        naveManager.combustible.currentAmmount += fuelRechargeAmmount;
        naveManager.combustible.currentAmmount = Mathf.Clamp(naveManager.combustible.currentAmmount, 0, naveManager.combustible.deposit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="NaveCentre")
        {
            NaveManager naveManager = other.gameObject.GetComponentInParent<NaveManager>();
            if(CheckShips(naveManager))
            {
                RechargeFuel(naveManager);
            }
            if (CheckpointManager.newest == this)
            {
                if(isFinal)
                {
                    GameManager.winner = naveManager;
                    GameManager.onRaceFinished.Invoke();
                }
                else
                {
                    GetComponentInChildren<RadarTarget>().radarImage = notNewestIcon;
                    CheckpointManager.OnCheckpointUnlocked.Invoke();
                }
            }
        }
    }
}
