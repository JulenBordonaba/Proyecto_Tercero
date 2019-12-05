using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static List<NaveManager> navesList = new List<NaveManager>();
    public static UnityEvent onRaceFinished;
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
    

    private void Awake()
    {
        winner = null;
        onRaceFinished = new UnityEvent();
        onRaceFinished.AddListener(FinishRace);
        navesList = new List<NaveManager>();
        List<GameObject> naves = new List<GameObject>();
        for (int i = 0; i < Global.numPlayers; i++)
        {
            if(i==0)
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
            for(int i=0;i<Global.numPlayers;i++)
            {
                scalers.Add(naves[i].GetComponentInChildren<CanvasScaler>());
            }

            if (screenDivision==ScreenDivision.Horizontal)
            {
                cam1.rect = new Rect(new Vector2(0, 0.5f), new Vector2(1, 0.5f));
                cam2.rect = new Rect(new Vector2(0, 0), new Vector2(1, 0.5f));

                foreach(CanvasScaler scaler in scalers)
                {
                    scaler.referenceResolution = new Vector2(1920,540);
                }
            }
            else if(screenDivision== ScreenDivision.Vertical)
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
    
    public static void TimeFinished()
    {
        if (GameManager.navesList.Count <= 0) return;
        foreach(NaveManager nm in GameManager.navesList)
        {
            nm.OnShipDestroyed();
        }
    }

    private void FinishRace()
    {
        GetComponent<Timer>().GetTime();
        UpdateScore();
        Global.winner = winner.GetComponent<InputManager>().numPlayer;
        SceneManager.LoadScene("Winner");
        //print(" ha ganado el jugador " + winner.GetComponent<InputManager>().numPlayer);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Global.numPlayers>1 && navesList.Count==1)
        {
            navesList[0].victoryImage.SetActive(true);
            PauseManager.inPause = true;
            GetComponent<Timer>().GetTime();
            //StartCoroutine(EndByElimination());
        }
    }

    IEnumerator EndByElimination()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("F&SMainMenu");

    }
}
