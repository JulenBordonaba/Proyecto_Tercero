using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Pon el prefab de la nave")]
    public GameObject navePrefab;
    [Tooltip("Pon el prefab de la nave enemiga")]
    public GameObject naveEnemigaPrefab;
    public List<Transform> spawns = new List<Transform>();

    private void Awake()
    {
        print(LayerMask.NameToLayer("Shield" + (1).ToString()));
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
            cam1.rect = new Rect(new Vector2(0, 0.5f), new Vector2(1, 0.5f));
            cam2.rect = new Rect(new Vector2(0, 0), new Vector2(1, 0.5f));

            //cam1.rect = new Rect(new Vector2(0, 0), new Vector2(0.5f, 1));
            //cam2.rect = new Rect(new Vector2(0.5f, 0), new Vector2(0.5f, 1));

            

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
