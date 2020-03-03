using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{

    public static EffectController current;



    private void Awake()
    {
        current = this;
    }
}
