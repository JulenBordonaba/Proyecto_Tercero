using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainMenu.currentMainMenu = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
