using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerScene : MonoBehaviour
{

    public Text timerText;
    public Text[] positionTexts;
    public GameObject trucoUnJugador;
    

    // Start is called before the first frame update
    void Start()
    {
        if(Global.numPlayers==1)
        {
            trucoUnJugador.SetActive(true);
        }

        for (int i = 0; i < positionTexts.Length; i++)
        {
            positionTexts[i].text = "Has quedado  " + Global.winners.Count + " º";
        }
        
        timerText.text = (TimeScore.currentScore.minutes < 10 ? "0" + TimeScore.currentScore.minutes : TimeScore.currentScore.minutes.ToString()) + "' " + (Mathf.FloorToInt(TimeScore.currentScore.seconds) < 10 ? "0" + Mathf.FloorToInt(TimeScore.currentScore.seconds) : Mathf.FloorToInt(TimeScore.currentScore.seconds).ToString()) + "''";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("MenuSubmit"))
        {
            SceneManager.LoadScene("OnlineMenu");
        }
    }
}
