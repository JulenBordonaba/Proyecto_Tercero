using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGSoundScript : MonoBehaviour
{
    //AudioSource myAudio;

    // Start is called before the first frame update
    void Start()
    {
        /*myAudio = GetComponent<AudioSource>();
        if (myAudio.mute == true)
        {
            myAudio.mute = false;
        }*/
    }

    //Play Global
    private static BGSoundScript instance = null;
    public static BGSoundScript Instance
    {
        get { return instance;  }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
