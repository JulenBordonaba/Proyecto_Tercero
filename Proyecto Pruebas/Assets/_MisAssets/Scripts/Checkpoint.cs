using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : Photon.PunBehaviour
{
    public float fuelRechargeAmmount = 20f;
    public bool isFinal = false;
    public GameObject checkpointGO;
    public GameObject newestIcon;
    public GameObject notNewestIcon;

    public Checkpoint[] conections;
    public bool isStartPoint = false;

    public List<NaveManager> passedShips = new List<NaveManager>();

    public byte classId { get; set; }

    public static object Deserialize(byte[] data)
    {
        Checkpoint result = new Checkpoint();
        result.classId = data[0];
        return result;
    }

    public static byte[] Serialize(object customType)
    {
        Checkpoint c = (Checkpoint)customType;
        return new byte[] { c.classId };
    }

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
            print(nm.gameObject.name + "    " + naveManager.gameObject.name);
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
        print("repairShield: " + naveManager.ShieldRepairRecharge);
        print("jumpBoost: " + naveManager.BoostJumpRecharge);
        foreach(Combustible c in naveManager.GetComponents<Combustible>())
        {
            switch(c.tipoCombustible)
            {
                case TipoCombustible.Turbo:
                    c.currentAmmount += naveManager.BoostJumpRecharge;
                    break;
                case TipoCombustible.Salto:
                    c.currentAmmount += naveManager.BoostJumpRecharge;
                    break;
                case TipoCombustible.Escudo:
                    c.currentAmmount += naveManager.ShieldRepairRecharge;
                    break;
                case TipoCombustible.Reparar:
                    c.currentAmmount += naveManager.ShieldRepairRecharge;
                    break;
                default:
                    break;
                
            }

            c.currentAmmount = Mathf.Clamp(c.currentAmmount, 0, c.deposit);
        }
    }
    [PunRPC]
    public void RegisterPosition(string nm)
    {
        Global.winners.Add(nm);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="NaveCentre")
        {
            NaveManager naveManager = other.gameObject.GetComponentInParent<NaveManager>();
            print(naveManager.gameObject.name);
            if(CheckShips(naveManager))
            {
                print("No había pasado");
                RechargeFuel(naveManager);
            }
            if (CheckpointManager.newest == this)
            {
                if(isFinal)
                {
                    GameManager.winner = naveManager;
                    photonView.RPC("RegisterPosition", PhotonTargets.AllBuffered, naveManager.GetComponent<PhotonView>().owner.NickName);
                    GameManager.CallOnRaceFinished(naveManager);
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
