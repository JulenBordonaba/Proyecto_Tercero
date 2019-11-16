using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Tooltip("Pon el GameObject del Canvas del menú de pausa")]
    public GameObject pauseMenuGameObject;
    [Tooltip("Pon el botón que aparecerá seleccionado")]
    public Button firstButton;
    public Toggle[] invertYToggle;

    public static bool inPause = false;
    public static bool[] invertY = new bool[2];

    private EventSystem evt;
    private GameObject sel;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        evt = EventSystem.current;
        for(int i=0;i<invertY.Length;i++)
        {
            invertY[i] = invertYToggle[i].isOn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(inPause)
        {
            KeepSelected();
        }

        if(inputManager.Pause())
        {
            if(inPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void InvertY(int player)
    {
        invertY[player-1] = invertYToggle[player-1].isOn;
    }

    private void KeepSelected()
    {
        if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject != sel)
            sel = evt.currentSelectedGameObject;
        else if (sel != null && evt.currentSelectedGameObject == null)
            evt.SetSelectedGameObject(sel);
    }

    public void Pause()
    {
        inPause = true;
        Time.timeScale = 0;
        pauseMenuGameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        evt.SetSelectedGameObject(firstButton.gameObject);
    }

    public void Resume()
    {
        inPause = false;
        Time.timeScale = 1;
        pauseMenuGameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
