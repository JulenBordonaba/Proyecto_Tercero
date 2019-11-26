using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int minutes = 0;
    public float seconds = 0;

    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        ShowTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (minutes < 0)
        {
            GameManager.TimeFinished();
        }
        Countdown();
        ShowTimer();
    }

    private void Countdown()
    {
        seconds -= Time.deltaTime;
        if(seconds<=0)
        {
            seconds = 60;
            minutes -= 1;
        }
    }

    private void ShowTimer()
    {
        if (minutes < 0) return;
            timerText.text = minutes.ToString() + ":" + (seconds < 10 ? "0" + Mathf.FloorToInt(seconds).ToString() : Mathf.FloorToInt(seconds).ToString());
    }

    
}
