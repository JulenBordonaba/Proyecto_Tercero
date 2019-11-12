﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Tooltip("Pon el botón que aparecerá seleccionado")]
    public Button firstButton;


    private EventSystem evt;
    private GameObject sel;

    private void Start()
    {
        evt = EventSystem.current;
        evt.SetSelectedGameObject(firstButton.gameObject);
        Cursor.lockState = CursorLockMode.Locked;
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
