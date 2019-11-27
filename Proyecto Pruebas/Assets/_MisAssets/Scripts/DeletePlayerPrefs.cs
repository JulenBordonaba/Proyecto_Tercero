using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlayerPrefs : MonoBehaviour
{
    [ContextMenu("Delete Player Prefs")]
    public void DeletePP()
    {
        PlayerPrefs.DeleteAll();
    }
}
