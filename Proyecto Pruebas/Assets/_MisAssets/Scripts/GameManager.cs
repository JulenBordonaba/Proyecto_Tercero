using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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

        List<GameObject> naves = new List<GameObject>();
        for (int i = 0; i < Global.numPlayers; i++)
        {
            if(i==0)
            {
                naves.Add(Instantiate(navePrefab, spawns[i].position, Quaternion.identity));
            }
            else
            {
                naves.Add(Instantiate(naveEnemigaPrefab, spawns[i].position, Quaternion.identity));
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
