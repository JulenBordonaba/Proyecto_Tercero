using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Tooltip("Pon el GameObject del Canvas del menú de pausa")]
    public GameObject pauseMenuGameObject;

    private bool inPause = false;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
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

    public void Pause()
    {
        inPause = true;
        Time.timeScale = 0;
        pauseMenuGameObject.SetActive(true);
    }

    public void Resume()
    {
        inPause = false;
        Time.timeScale = 1;
        pauseMenuGameObject.SetActive(false);
    }
}
