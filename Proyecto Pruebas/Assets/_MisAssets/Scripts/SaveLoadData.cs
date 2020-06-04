using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadData : Singleton<SaveLoadData>
{




    public void Start()
    {
        OptionsMenu.settings = Global.LoadData<Settings>("OptionSettings.json");
        if(OptionsMenu.settings==null)
        {
            OptionsMenu.settings = new Settings();
        }
    }


    private void OnApplicationQuit()
    {
        OptionsMenu.settings.SaveData<Settings>("OptionSettings.json");
    }
}
