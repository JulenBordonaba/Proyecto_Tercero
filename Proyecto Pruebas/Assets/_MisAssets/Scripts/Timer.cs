using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{

    [System.Serializable]
    public class TimerTime
    {
        public int minutes = 0;
        public float seconds = 0;
    }

    [System.Serializable]
    public class TimeEvent
    {
        [SerializeField]
        public TimerTime eventTime = new TimerTime();
        [SerializeField]
        public UnityEvent onTimeReached = new UnityEvent();
        public bool called = false;
    }

    [SerializeField]
    public TimerTime currentTime;
    
    
    

    public List<TimeEvent> timeEvents = new List<TimeEvent>();

    public UnityEvent OnTimeFinished = new UnityEvent();

    public bool onlyRoomMaster = false;

    public bool freezed = false;

    private int startMinutes;
    private float startSeconds;

    // Start is called before the first frame update
    void Start()
    {
        startMinutes = currentTime.minutes;
        startSeconds = currentTime.seconds;
        //ShowTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(onlyRoomMaster)
        {
            if(!PhotonNetwork.isMasterClient)
            {
                return;
            }
        }
        if (freezed) return;

        if (currentTime.minutes < 0)
        {
            OnTimeFinished.Invoke();
            freezed = true;
            //GameManager.TimeFinished();
        }
        Countdown();
        CheckTimeEvents();
        //ShowTimer();
    }

    public void CheckTimeEvents()
    {
        foreach(TimeEvent te in timeEvents)
        {
            if(!te.called)
            {
                if(currentTime.minutes<=te.eventTime.minutes && currentTime.seconds <= te.eventTime.seconds)
                {
                    te.called = true;
                    print("Llama a el evento");
                    te.onTimeReached.Invoke();
                }
            }
        }
    }

    public void GetTime()
    {
        float startTime = (startMinutes * 60) + startSeconds;
        float leftTime = startTime - ((currentTime.minutes * 60) + currentTime.seconds);
        TimeScore.currentScore = TimeScore.TimeToScore(leftTime);
    }
    

    private void Countdown()
    {
        if (freezed) return;
        currentTime.seconds -= Time.deltaTime;
        if(currentTime.seconds <= 0)
        {
            currentTime.seconds = 60;
            currentTime.minutes -= 1;
        }
    }

    public void Activate()
    {
        freezed = false;
    }

    public void Stop()
    {
        freezed = true;
    }

    
}

