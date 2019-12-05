using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCamera : MonoBehaviour
{
    public float waitTime = 1;
    public GameObject exitImage;

    private bool canExit = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExitButton());
    }

    // Update is called once per frame
    void Update()
    {
        if(canExit)
        {
            if(Input.GetButtonDown("MenuSubmit"))
            {

                SceneManager.LoadScene("F&SMainMenu");
            }
        }
    }

    private IEnumerator ExitButton()
    {
        yield return new WaitForSeconds(waitTime);
        canExit = true;
        exitImage.SetActive(true);
    }

}
