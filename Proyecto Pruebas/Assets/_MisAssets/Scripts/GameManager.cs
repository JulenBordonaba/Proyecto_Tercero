using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Photon.PunBehaviour
{
    public static GameManager current;

    public static List<NaveManager> navesList = new List<NaveManager>();
    public static event Action<NaveManager> OnRaceFinished;
    public static NaveManager winner;
    public static TimeScore[] records = new TimeScore[3];


    [Tooltip("Pon el prefab de la nave")]
    public GameObject navePrefab;
    [Tooltip("Pon el prefab de la nave enemiga")]
    public GameObject naveEnemigaPrefab;
    [Tooltip("Pon los puntos de spawn de las naves")]
    public List<Transform> spawns = new List<Transform>();
    [Tooltip("Pon el eje en el que se divide la pantalla cuando hay 2 jugadores")]
    public ScreenDivision screenDivision;

    public EffectData prestartEffect;

    public List<string> winners = new List<string>();

    public TextPopUp popUp;

    public GameObject hangar;


    private Timer timer;
    

    private void Awake()
    {
        current = this;
        Global.winners = new List<string>();
        timer = GetComponent<Timer>();
        if(PhotonNetwork.connected)
        {

            
            
        }
        else
        {

            winner = null;
            OnRaceFinished += FinishRace;
            navesList = new List<NaveManager>();
            List<GameObject> naves = new List<GameObject>();
            for (int i = 0; i < Global.numPlayers; i++)
            {
                if (i == 0)
                {
                    naves.Add(Instantiate(navePrefab, spawns[i].position, Quaternion.identity));
                    naves[i].GetComponentInChildren<NaveController>().modelTransform.rotation = Quaternion.Euler(0, spawns[i].eulerAngles.y, 0);
                }
                else
                {
                    naves.Add(Instantiate(naveEnemigaPrefab, spawns[i].position, Quaternion.identity));

                    naves[i].GetComponentInChildren<NaveController>().modelTransform.rotation = Quaternion.Euler(0, spawns[i].eulerAngles.y, 0);
                }
            }

            if (Global.numPlayers > 1)
            {
                Camera cam1 = naves[0].GetComponentInChildren<Camera>();
                Camera cam2 = naves[1].GetComponentInChildren<Camera>();

                naves[1].GetComponentInChildren<AudioListener>().enabled = false;

                List<CanvasScaler> scalers = new List<CanvasScaler>();
                for (int i = 0; i < Global.numPlayers; i++)
                {
                    scalers.Add(naves[i].GetComponentInChildren<CanvasScaler>());
                }

                if (screenDivision == ScreenDivision.Horizontal)
                {
                    cam1.rect = new Rect(new Vector2(0, 0.5f), new Vector2(1, 0.5f));
                    cam2.rect = new Rect(new Vector2(0, 0), new Vector2(1, 0.5f));

                    foreach (CanvasScaler scaler in scalers)
                    {
                        scaler.referenceResolution = new Vector2(1920, 540);
                    }
                }
                else if (screenDivision == ScreenDivision.Vertical)
                {
                    cam1.rect = new Rect(new Vector2(0, 0), new Vector2(0.5f, 1));
                    cam2.rect = new Rect(new Vector2(0.5f, 0), new Vector2(0.5f, 1));

                    foreach (CanvasScaler scaler in scalers)
                    {
                        scaler.referenceResolution = new Vector2(960, 1080);
                    }
                }





            }
        }
    }

    public void MakeRoomPrivate()
    {

        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;

        PhotonNetwork.room.open = false;
        PhotonNetwork.room.visible = false;
    }

    public static void CallOnRaceFinished(NaveManager nm)
    {
        OnRaceFinished.Invoke(nm);
    }

    public void StartRace()
    {
        photonView.RPC("StartRaceRPC", PhotonTargets.All);
    }

    [PunRPC]
    public void StartRaceRPC()
    {
        timer.Activate();

        hangar.SetActive(false);

        foreach(NaveManager nm in navesList)
        {
            nm.effectManager.StopEffect(prestartEffect.id);
        }
    }


    public void StartGame()
    {
        winner = null;
        OnRaceFinished += FinishRace;
        navesList = new List<NaveManager>();
        List<GameObject> naves = new List<GameObject>();
        if (PhotonNetwork.playerList.Length <= spawns.Count)
        {
            naves.Add(PhotonNetwork.Instantiate("NaveOnline" + Global.myShipType, spawns[PhotonNetwork.playerList.Length - 1].position, Quaternion.identity, 0, null));
        }
        else
        {
            naves.Add(PhotonNetwork.Instantiate("NaveOnline" + Global.myShipType, spawns[spawns.Count - 1].position, Quaternion.identity, 0, null));
        }
    }

    

    public void UpdateScore()
    {
        TimeScore aux = new TimeScore();
        bool newRecord = false;
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey("record" + (i + 1).ToString()))
            {
                if(newRecord)
                {
                    TimeScore aux2 = records[i];
                    records[i] = aux;
                    aux = aux2;

                }
                else if(TimeScore.ScoreToTime(TimeScore.currentScore)<TimeScore.ScoreToTime(records[i]) || TimeScore.ScoreToTime(records[i])==-1)
                {
                    newRecord = true;
                    aux = records[i];
                    records[i] = TimeScore.currentScore;
                }
                
            }
            PlayerPrefs.SetFloat(("record" + (i + 1).ToString()), TimeScore.ScoreToTime(records[i]));
        }
    }
    
    public void TimeFinished()
    {
        if (GameManager.navesList.Count <= 0) return;
        foreach(NaveManager nm in GameManager.navesList)
        {
            nm.OnShipDestroyed();
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void FinishRace(NaveManager nm)
    {
        print(Global.winners[Global.winners.Count - 1] + " " + PhotonNetwork.player.NickName);
        if(PhotonNetwork.player.NickName == Global.winners[Global.winners.Count-1])
        {
            navesList.Remove(nm);
            timer.GetTime();
            UpdateScore();
            Global.winner = winner.GetComponent<InputManager>().numPlayer;
            if (PhotonNetwork.inRoom)
            {
                LeaveRoom();
            }
            SceneManager.LoadScene("Winner");
            //print(" ha ganado el jugador " + winner.GetComponent<InputManager>().numPlayer);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.connected)
        {
            CheckPositions();

            winners = Global.winners;
        }
        else
        {
            if (Global.numPlayers > 1 && navesList.Count == 1)
            {
                navesList[0].victoryImage.SetActive(true);
                PauseManager.inPause = true;
                GetComponent<Timer>().GetTime();
                //StartCoroutine(EndByElimination());
            }
        }
        
    }

    public void CheckPositions()
    {
        List<NaveManager> navePositions = navesList;
        for (int j = 0; j < navePositions.Count; j++)
        {
            for (int i = 0; i < navePositions.Count - 1; i++)
            {
                if (navePositions[i].DistanceToNextCheckpoint > navePositions[i + 1].DistanceToNextCheckpoint)
                {
                    NaveManager aux = navePositions[i + 1];
                    navePositions[i + 1] = navePositions[i];
                    navePositions[i] = aux;
                }
            }
        }
        for (int i = 0; i < navePositions.Count; i++)
        {
            navePositions[i].position = i+Global.winners.Count+1;
        }

    }

    IEnumerator EndByElimination()
    {
        yield return new WaitForSeconds(5f);

        if(PhotonNetwork.inRoom)
        {
            LeaveRoom();
        }
        SceneManager.LoadScene("OnlineMenu");

    }
}
