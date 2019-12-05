using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer i;

    public int minutes = 0;
    public float seconds = 0;

    public Text timerText;

    private int startMinutes;
    private float startSeconds;

    // Start is called before the first frame update
    void Start()
    {
        i = this;
        startMinutes = minutes;
        startSeconds = seconds;
        //ShowTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (minutes < 0)
        {
            GameManager.TimeFinished();
        }
        Countdown();
        //ShowTimer();
    }

    public void GetTime()
    {
        float startTime = (startMinutes * 60) + startSeconds;
        float leftTime = startTime - ((minutes * 60) + seconds);
        TimeScore.currentScore = TimeScore.TimeToScore(leftTime);
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
