using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combustible : MonoBehaviour
{
    public Color color;     //color del combustible
    public float deposit;   //cantidad máxima de combustible
    public float pasiveConsumption;     //cantidad de combustoble que se gasta cuando se usa activamente
    public float activeConsumption;     //cantidad de combustible que se gasta pasivamente
    public float duration;      //duración de la acción del combustible

    protected float currentAmmount;     //cantidad actual de combustible

    //se llama a esta función cuando se usa el combustible
    public abstract void Use();

}
