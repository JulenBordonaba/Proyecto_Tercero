using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTypeVolume : MonoBehaviour
{
    public AudioSource audioSource;

    public SoundType soundType;

    private float defaultVolume;

    private void Awake()
    {
        defaultVolume = audioSource.volume;
    }
    
    // Update is called once per frame
    void Update()
    {
        audioSource.volume = FinalVolume;
    }

    public float FinalVolume
    {
        get
        {
            if(soundType== SoundType.effect)
            {
                return defaultVolume * OptionsMenu.effectVolume * OptionsMenu.generalVolume;
            }
            else if (soundType== SoundType.music)
            {
                return defaultVolume * OptionsMenu.musicVolume * OptionsMenu.generalVolume;
            }
            return defaultVolume;
        }
    }


}
