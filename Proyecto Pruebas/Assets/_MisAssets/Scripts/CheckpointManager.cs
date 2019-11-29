using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    public static Checkpoint newest;
    public static UnityEvent OnCheckpointUnlocked = new UnityEvent();
    public static int currentCheckpoint = 0;
    public static int numCheckpoints;

    // Start is called before the first frame update
    void Awake()
    {
        currentCheckpoint = 0;
        numCheckpoints = checkpoints.Count;
        checkpoints[currentCheckpoint].Unlock();
        OnCheckpointUnlocked = new UnityEvent();
        OnCheckpointUnlocked.AddListener(UnlockCheckpoint);

    }

    private void UnlockCheckpoint()
    {
        currentCheckpoint += 1;
        checkpoints[currentCheckpoint].Unlock();
    }

    
    
}
