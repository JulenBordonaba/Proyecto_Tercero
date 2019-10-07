using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combustible : MonoBehaviour
{
    public Color color;
    public float deposit;
    public float pasiveConsumption;
    public float activeConsumption;
    public float duration;

    public abstract void Use();

}
