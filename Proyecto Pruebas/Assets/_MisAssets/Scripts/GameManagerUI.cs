using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{

    public Text contadorCheckpointsText;
    public Text timerText;
    public Timer gameTimer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        contadorCheckpointsText.text = (CheckpointManager.currentCheckpoint - 1).ToString() + " / " + (CheckpointManager.numCheckpoints - 1).ToString();
        ShowTimer();
    }

    private void ShowTimer()
    {
        if (gameTimer.currentTime.minutes < 0) return;
        timerText.text = gameTimer.currentTime.minutes.ToString() + "' " + (gameTimer.currentTime.seconds < 10 ? "0" + Mathf.FloorToInt(gameTimer.currentTime.seconds).ToString() : Mathf.FloorToInt(gameTimer.currentTime.seconds).ToString()) + "''";
    }

}
