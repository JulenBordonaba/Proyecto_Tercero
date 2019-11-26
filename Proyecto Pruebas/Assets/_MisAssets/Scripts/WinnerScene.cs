using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerScene : MonoBehaviour
{

    public Text winnerText;
    public string winnerMessage;
    

    // Start is called before the first frame update
    void Start()
    {
        if(Global.numPlayers==1)
        {
            winnerText.text = winnerMessage;
        }
        else
        {
            winnerText.text = "Ha ganado el jugador " + Global.winner;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("MenuSubmit"))
        {
            SceneManager.LoadScene("F&SMainMenu");
        }
    }
}
