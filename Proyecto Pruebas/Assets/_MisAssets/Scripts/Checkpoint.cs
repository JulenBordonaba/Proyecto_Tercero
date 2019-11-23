using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isFinal = false;
    public GameObject checkpointGO;

    private List<NaveManager> passedShips = new List<NaveManager>();

    public void Unlock()
    {
        CheckpointManager.newest = this;
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
            if (CheckpointManager.newest == this)
            {
                if(isFinal)
                {
                    GameManager.winner = naveManager;
                    GameManager.OnRaceFinished.Invoke();
                }
                else
                {
                    CheckpointManager.OnCheckpointUnlocked.Invoke();
                }
            }
        }
    }
}
