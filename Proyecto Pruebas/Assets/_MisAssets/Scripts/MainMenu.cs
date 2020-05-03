using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MainMenu : Photon.PunBehaviour
{
    public List<Text> recordTexts = new List<Text>();

    public TMP_InputField nameInput;

    public Color selectedColor;
    public Color unselectedColor;

    private EventSystem evt;
    private GameObject sel;

    private void Awake()
    {
        LoadScores();
        
    }

    private void Start()
    {
        ScoresToTexts();
        nameInput.text = PhotonNetwork.player.NickName;
        Global.myShipType = "Scavenger";
        Global.onePlayer = false;
        evt = EventSystem.current;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    

    public void OnNameChanged()
    {
        PhotonNetwork.playerName = nameInput.text;
        PlayerPrefs.SetString("Nickname", PhotonNetwork.player.NickName);
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

        print("ID: " + PhotonNetwork.player.UserId);
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

    public void SetShip(string tn)
    {
        Debug.Log("LLAMA A LA NAVE");
        Global.myShipType = tn;
    }

    public void ClassSelected(Image i)
    {
        i.color = selectedColor;
    }

    public void ClassUnselected(Image i)
    {
        i.color = unselectedColor;
    }

    
}
