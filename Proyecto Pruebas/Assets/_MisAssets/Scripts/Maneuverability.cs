using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maneuverability : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Asigna la velocidad maxima que podra alcanzar")]
    public float velocity;     //Velocidad maxipa que alcanzara la nave
    [Tooltip("Asigna la aceleracion del objeto")]
    public float acceleration;      //Aceleracion de la nave
    [Tooltip("Asigna el ratio de manejo")]
    public float maneuver;      //El manejo afecta a como gira y derrapa la nave
    [Tooltip("Asigna la distancia que se recorrera en un dash")]
    public float dash;      //Distancia que se recorrera en un dash
    [Tooltip("Asigna el bonus de velocidad que otorgara estar en Rebufo")]
    public float recoil;        //bonus de velocidad del rebufo
    [Tooltip("Asigna el bonus de velocidad que otorgara estar en Turbo")]
    public float turbo;        //bonus de velocidad del turbo
    [Header("Constantes")]
    [Tooltip("Pon el multiplicador a la velocidad de la nave")]
    public float velocityMultiplier;    //multiplicador a la velocidad de la nave, sirve para mantener las estadísticas en un rango de 0-100 y poder aumentar la velocidad de las naves
    public float velocityWeightInfluence;
    public float accelerationWeightInfluence;

    
    public float currentVelocity { get; set; }     //Velocidad maxima actual que alcanzara la nave
    public float currentAcceleration { get; set; }        //Aceleracion actual de la nave
    public float currentManeuver { get; set; }        //El manejo  actual de la nave
    public float currentDash { get; set; }        //Dash actual de la nave
    public float currentRecoil { get; set; }          //bonus de velocidad del rebufo actual
    public float currentTurbo { get; set; }          //bonus de velocidad del turbo actual





    public float MaxVelocity    //devuelve la velocidad máxima de la nave sin aplicar modificadores por posición, rebufo, turbo y salud
    {
        get { return VelocityWithWeight * velocityMultiplier; }
    }

    public float VelocityWithWeight //devuelve la velocidad base de la nave afectada por el peso
    {
        get { return currentVelocity - (GetComponent<Stats>().actualWeight * velocityWeightInfluence); }
    }

    public float AcelerationWithWeight  //devuelve la aceleración de la nave afectada por el peso
    {
        get { return currentAcceleration - (accelerationWeightInfluence * GetComponent<Stats>().actualWeight); }
    }

}
