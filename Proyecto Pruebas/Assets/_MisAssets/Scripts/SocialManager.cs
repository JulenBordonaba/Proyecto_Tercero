using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SocialManager : MonoBehaviour
{
    public GameObject socialCanvas;

    public GameObject socialFirstUIElement;
    public GameObject mainMenuFirstUIElement;

    private EventSystem evt;

    // Start is called before the first frame update
    void Start()
    {
        evt = EventSystem.current;

    }

    // Update is called once per frame
    void Update()
    {
        ChangeMenu();
    }

    void ChangeMenu()
    {
        if(Input.GetKeyDown(KeyCode.Joystick1Button13) || Input.GetKeyDown(KeyCode.Tab))
        {
            if(socialCanvas.activeInHierarchy)
            {
                socialCanvas.SetActive(false);
                evt.SetSelectedGameObject(mainMenuFirstUIElement);
            }
            else
            {
                socialCanvas.SetActive(true);
                evt.SetSelectedGameObject(socialFirstUIElement);
            }
        }
    }
}
