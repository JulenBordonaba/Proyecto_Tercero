using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Combustible : MonoBehaviour
{
    [Tooltip("Elige una de las 4 opciones")]
    public TipoCombustible tipoCombustible;     //tipo del combustible
    [Tooltip("Elige el color del combustible")]
    public Color color;     //color del combustible
    [Tooltip("Pon la cantidad máxima de combustible")]
    public float deposit;   //cantidad máxima de combustible
    [Tooltip("Asigna la cantidad de combustible que gasta la nave mientras este combustible esta activo")]
    public float pasiveConsumption;     //cantidad de combustible que se gasta pasivamente cada 5 segundos
    [Tooltip("Asigna la cantidad de combustible que gasta la nave cada vez que se usa este combustible")]
    public float activeConsumption;     //cantidad de combustoble que se gasta cuando se usa activamente
    [Tooltip("Asigna los segundos que dura la habilidad del combustible")]
    public float duration;      //duración de la acción del combustible


    public float currentAmmount;    //cantidad actual de combustible
    

    private void Start()
    {
        currentAmmount = deposit;
    }
    

    public void PasiveConsumption()
    {
        currentAmmount -= (pasiveConsumption * Time.deltaTime) / 5;
    }



}
