using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EffectIcon : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI durationText;

    private void Awake()
    {
        if(!icon)
        {
            icon = GetComponentInChildren<Image>();
        }
        if(!durationText)
        {
            durationText = GetComponent<TextMeshProUGUI>();
        }
    }


}
