using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool newest = false;
    public bool isFinal = false;
    public GameObject checkpointGO;

    private List<NaveManager> passedShips = new List<NaveManager>();

    public void Unlock()
    {
        newest = true;
        checkpointGO.SetActive(true);
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
            if (newest)
            {
                if(isFinal)
                {
                    GameManager.winner = naveManager;
                    GameManager.OnRaceFinished.Invoke();
                }
                else
                {
                    newest = false;
                    CheckpointManager.OnCheckpointUnlocked.Invoke();
                }
            }
        }
    }
}
