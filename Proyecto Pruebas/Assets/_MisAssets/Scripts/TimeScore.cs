using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScore
{

    public static TimeScore currentScore;

    public int minutes;
    public float seconds;

    public TimeScore(int _minutes, float _seconds)
    {
        minutes = _minutes;
        seconds = _seconds;
    }

    public TimeScore()
    {
        minutes = 0;
        seconds = 0;
    }

    public static TimeScore TimeToScore(float time)
    {
        int _minutes = Mathf.FloorToInt(time / 60);
        float _seconds = time - (_minutes * 60);
        TimeScore _score = new TimeScore(_minutes, _seconds);
        return _score;
    }

    public static float ScoreToTime(TimeScore _score)
    {
        return ((_score.minutes * 60) + _score.seconds);
    }
}
