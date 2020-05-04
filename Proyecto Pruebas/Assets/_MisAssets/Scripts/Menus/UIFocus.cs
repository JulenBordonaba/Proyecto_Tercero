using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIFocus : MonoBehaviour, IPointerDownHandler
{
    public Button defaultButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("vuelve a coger el foco");
        defaultButton.Select();
    }
}
