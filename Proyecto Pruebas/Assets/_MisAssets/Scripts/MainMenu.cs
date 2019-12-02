using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public List<Text> recordTexts = new List<Text>();

    private EventSystem evt;
    private GameObject sel;

    private void Awake()
    {
        LoadScores();
        
    }

    private void Start()
    {
        ScoresToTexts();
        evt = EventSystem.current;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    private void LoadScores()
    {
        for(int i=0;i<3;i++)
        {
            if (PlayerPrefs.HasKey("record" + (i + 1).ToString()))
            {
                GameManager.records[i] = TimeScore.TimeToScore(PlayerPrefs.GetFloat("record" + (i + 1).ToString()));
            }
            else
            {
                GameManager.records[i] = TimeScore.TimeToScore(-1);
                PlayerPrefs.SetFloat(("record" + (i + 1).ToString()), TimeScore.ScoreToTime(GameManager.records[i]));
            }
        }
        
    }

    private void ScoresToTexts()
    {
        for(int i=0;i<recordTexts.Count;i++)
        {
            if(TimeScore.ScoreToTime(GameManager.records[i])==-1)
            {
                recordTexts[i].text = (i + 1).ToString() + "º   --' --''";
            }
            else
            {
                recordTexts[i].text = (i + 1).ToString() + "º   " + (GameManager.records[i].minutes<10 ? "0" + GameManager.records[i].minutes : GameManager.records[i].minutes.ToString()) + "' " + (Mathf.FloorToInt(GameManager.records[i].seconds)<10 ? "0" + Mathf.FloorToInt(GameManager.records[i].seconds) : Mathf.FloorToInt(GameManager.records[i].seconds).ToString()) + "''";
            }
        }
    }

    private void Update()
    {
        KeepSelected();
    }

    private void KeepSelected()
    {
        if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject != sel)
            sel = evt.currentSelectedGameObject;
        else if (sel != null && evt.currentSelectedGameObject == null)
            evt.SetSelectedGameObject(sel);
    }

    public void SetPlayers(int numPlayers)
    {
        Global.numPlayers = numPlayers;
        print(Global.numPlayers);
    }
    
}
