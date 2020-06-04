using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UISelect : EventTrigger
{

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        // Call methods when the UI element is selected
        if(GetComponent<Animator>())
        GetComponent<Animator>().SetBool("Selected",true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        // Call methods when the UI element is deselected
        if (GetComponent<Animator>())
            GetComponent<Animator>().SetBool("Selected",false);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!Cursor.visible || Cursor.lockState == CursorLockMode.Locked) return;
        else base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!Cursor.visible || Cursor.lockState == CursorLockMode.Locked) return;
        else base.OnPointerExit(eventData);
    }
}
