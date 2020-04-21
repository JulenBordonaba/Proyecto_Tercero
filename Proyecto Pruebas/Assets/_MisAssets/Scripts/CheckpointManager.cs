using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckpointManager : Photon.PunBehaviour
{

    public static CheckpointManager current;

    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    public GameObject SpawnsGameObject;
    public int rechargeAmmount = 4;
    public static Checkpoint newest;
    public static UnityEvent OnCheckpointUnlocked = new UnityEvent();
    public static int currentCheckpoint = 0;
    public static int numCheckpoints;
    public static bool isCircuit = false;

    public int checkpointNumber = 5;

    public List<Checkpoint> circuit = new List<Checkpoint>();



    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCheckpoint = 1;
        if(PhotonNetwork.isMasterClient)
        {
            GenerateCircuit(checkpointNumber);

            print("Circuito pre rpc: " + CircuitToString(circuit));

            photonView.RPC("SetCircuit", PhotonTargets.AllBuffered, CircuitToString(circuit));
        }
        
        

    }

    public string CircuitToString(List<Checkpoint> _circuit)
    {
        string s = "";
        for (int i = 0; i < circuit.Count; i++)
        {
            for (int j = 0; j < checkpoints.Count; j++)
            {
                if(circuit[i]==checkpoints[j])
                {
                    if(i>=circuit.Count-1)
                    {
                        s += j.ToString();
                    }
                    else
                    {
                        s += j.ToString() + ",";
                    }
                    break;
                }
            }
        }
        return s;
    }

    public List<Checkpoint> StringToCircuit(string _circuit)
    {
        string[] checkpointIndexes = _circuit.Split(new char[] {','});
        List<Checkpoint> newCircuit = new List<Checkpoint>();
        foreach(string s in checkpointIndexes)
        {
            newCircuit.Add(checkpoints[int.Parse(s)]);
        }
        return newCircuit;
    }


    

    [PunRPC]
    public void SetCircuit(string _circuit)
    {
        print("circuito post RPC: " + _circuit);

        circuit =StringToCircuit(_circuit);
        foreach (Checkpoint c in circuit)
        {
            c.isFinal = false;
        }
        circuit[circuit.Count - 1].isFinal = true;
        numCheckpoints = circuit.Count;
        circuit[currentCheckpoint].Unlock();
        OnCheckpointUnlocked = new UnityEvent();
        OnCheckpointUnlocked.AddListener(UnlockCheckpoint);
        SpawnsGameObject.transform.position = circuit[0].transform.position;
        GameManager.current.StartGame();
    }

    public void GenerateCircuit(int _checkpointNumber)
    {
        isCircuit = false;
        circuit = new List<Checkpoint>();
        Checkpoint first = SetStartPoint();

        if (first == null) return;

        circuit.Add(first);
        bool hasToReset = false;
        for(int i=1;i<_checkpointNumber+1;i++)
        {
            if(!SetNewPoint(circuit[circuit.Count-1]))
            {
                hasToReset = true;
                break;
            }
        }
        if(hasToReset)
        {
            GenerateCircuit(_checkpointNumber);
        }

        foreach(Checkpoint c in circuit)
        {
            c.isFinal = false;
        }
        circuit[circuit.Count - 1].isFinal = true;
        

    }
    

    public bool SetNewPoint(Checkpoint current)
    {
        Checkpoint newCheckpoint = null;
        do
        {
            bool canContinue = false;
            foreach(Checkpoint c in current.conections)
            {
                if(!circuit.Contains(c))
                {
                    canContinue = true;
                    break;
                }
            }

            if(!canContinue)
            {
                return false;
            }
            newCheckpoint = current.conections[Random.Range(0, current.conections.Length)];
            
        } while (circuit.Contains(newCheckpoint));
        circuit.Add(newCheckpoint);
        return true;
    }

    public Checkpoint SetStartPoint()
    {
        List<Checkpoint> startPoints = new List<Checkpoint>();
        foreach (Checkpoint c in checkpoints)
        {
            if (c.isStartPoint)
            {
                startPoints.Add(c);
            }
        }

        if(startPoints.Count>0)
        {
            return startPoints[Random.Range(0, startPoints.Count)];
        }
        else
        {
            return null;
        }
    }

    private void UnlockCheckpoint()
    {
        currentCheckpoint += 1;
        circuit[currentCheckpoint].Unlock();
    }

    public Checkpoint CurrentCheckpoint
    {
        get { return circuit[currentCheckpoint]; }
    }
    
}
