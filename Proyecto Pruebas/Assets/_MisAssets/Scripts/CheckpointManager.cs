using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    public static Checkpoint newest;
    public static UnityEvent OnCheckpointUnlocked = new UnityEvent();
    private int currentCheckpoint = 0;

    // Start is called before the first frame update
    void Awake()
    {
        currentCheckpoint = 0;
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
