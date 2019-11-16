using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{


    private EventSystem evt;
    private GameObject sel;

    private void Start()
    {
        evt = EventSystem.current;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
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
