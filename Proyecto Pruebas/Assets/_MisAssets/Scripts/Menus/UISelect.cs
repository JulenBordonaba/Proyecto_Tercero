using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UISelect : EventTrigger
{

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        // Call methods when the UI element is selected
        GetComponent<Animator>().SetBool("Selected",true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        // Call methods when the UI element is deselected
        GetComponent<Animator>().SetBool("Selected",false);
    }
}
