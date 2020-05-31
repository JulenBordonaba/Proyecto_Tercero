using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMoveButton : MonoBehaviour
{
    public Slider slider;


    public void ChangeSliderValue(float offset)
    {
        slider.value += offset;
    }
}
