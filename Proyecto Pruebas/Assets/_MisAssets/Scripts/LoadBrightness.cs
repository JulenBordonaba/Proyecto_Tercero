using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBrightness : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.ambientLight = new Color(OptionsMenu.settings.brightness, OptionsMenu.settings.brightness, OptionsMenu.settings.brightness, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
