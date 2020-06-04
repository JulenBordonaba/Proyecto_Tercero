using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static bool inverted = false;
    public static float generalVolume = 1;
    public static float musicVolume = 1;
    public static float effectVolume = 1;
    public static float sensitivity = 10f;

    [Header("Graphics")]
    public Slider brightnessSlider;
    public Slider vSyncSlider;
    public Slider antialiasingSlider;
    public Slider textureQualitySlider;
    public Slider shadowResolutionSlider;

    [Header("Controller")]
    public Slider sensitivitySlider;
    public Slider invertControlsSlider;
    public Slider controllerSlider;

    [Header("Music")]
    public Slider generalVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectVolumeSlider;

    public Dictionary<int, bool> intToBool = new Dictionary<int, bool>()
    {
        {0,false },
        {1,true }
    };

    public Dictionary<bool, int> boolToInt = new Dictionary<bool, int>()
    {
        {false,0 },
        {true,1 }
    };

    

    // Start is called before the first frame update
    void Start()
    {

        SetSliderValues();

    }

    public void OnEnable()
    {
        SetSliderValues();
    }

    public void SetSliderValues()
    {
        //sonido
        musicVolumeSlider.value = musicVolume;
        effectVolumeSlider.value = effectVolume;
        generalVolumeSlider.value = generalVolume;

        //graphics
        vSyncSlider.value = QualitySettings.vSyncCount;
        brightnessSlider.value = RenderSettings.ambientLight.r;
        antialiasingSlider.value = QualitySettings.antiAliasing / 2f;
        textureQualitySlider.value = QualitySettings.masterTextureLimit;
        shadowResolutionSlider.value = (int)QualitySettings.shadowResolution;

        //controller
        controllerSlider.value = (int)InputManager.controllerType;
        invertControlsSlider.value = boolToInt[inverted];
        sensitivitySlider.value = sensitivity * 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Music

    public void ChangeMusicVolume()
    {
        musicVolume = musicVolumeSlider.value ;
    }

    public void ChangeEffectVolume()
    {
        effectVolume = effectVolumeSlider.value ;
    }

    public void ChangeGeneralVolume()
    {
        generalVolume = generalVolumeSlider.value ;
    }

    #endregion

    #region Controller

    public void ChangeSensitivity()
    {
        sensitivity = sensitivitySlider.value/10f;
    }

    public void ChooseController()
    {
        InputManager.controllerType = (ControllerType)controllerSlider.value;
    }

    public void Inverted()
    {
        inverted = intToBool[(int)invertControlsSlider.value];
    }

    #endregion

    #region Graphics

    public void TextureQuality()
    {
        QualitySettings.masterTextureLimit = (int)textureQualitySlider.value;
    }

    public void ShadowResolution()
    {
        QualitySettings.shadowResolution = (UnityEngine.ShadowResolution)shadowResolutionSlider.value;
    }

    public void Brightness()
    {
        RenderSettings.ambientLight = new Color(brightnessSlider.value, brightnessSlider.value, brightnessSlider.value, 1.0f);
    }

    public void VSync()
    {
            QualitySettings.vSyncCount = (int)vSyncSlider.value;
    }

    public void AntiAliasing(bool active)
    {
        QualitySettings.antiAliasing = (int)antialiasingSlider.value * 2;
    }

    #endregion

}
