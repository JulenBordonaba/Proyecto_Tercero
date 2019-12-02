using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Sprite loadingSprite;
    public Sprite loadedSprite;
    public Image loadingImage;

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadSceneAsync(string scene)
    {
        StartCoroutine(LoadYourAsyncScene(scene));
    }
    IEnumerator LoadYourAsyncScene(string scene)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        asyncLoad.allowSceneActivation = false;

        loadingImage.sprite = loadingSprite; ;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {

            // Check if the load has finished
            if (asyncLoad.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                loadingImage.sprite = loadedSprite;
                //Wait to you press the space key to activate the Scene
                if (Input.GetButtonDown("MenuSubmit"))
                    //Activate the Scene
                    asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
