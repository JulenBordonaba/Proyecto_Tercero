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
    public float boost;        //bonus de velocidad del turbo
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
    public float currentBoost { get; set; }          //bonus de velocidad del turbo actual


    private void Awake()
    {
        //al inicializarse el objeto los valores actuales son iguales al doble de los totales
        currentVelocity = velocity;
        currentAcceleration = acceleration;
        currentManeuver = maneuver;
        currentDash = dash;
        currentRecoil = recoil;
        currentBoost = boost;
    }

    public void AddPieceValues(float importance)
    {
        currentVelocity += velocity * (importance / 100);
        currentAcceleration += acceleration * (importance / 100);
        currentManeuver += maneuver * (importance / 100);
        currentDash += dash * (importance / 100);
        currentRecoil += recoil * (importance / 100);
        currentBoost += boost * (importance / 100);
    }

    public void OnPieceDestroyed(float importance)
    {
        currentVelocity -= velocity * (importance/100);
        currentAcceleration -= acceleration * (importance / 100);
        currentManeuver -= maneuver * (importance / 100);
        currentDash -= dash * (importance / 100);
        currentRecoil -= recoil * (importance / 100);
        currentBoost -= boost * (importance / 100);
    }


    public float MaxVelocity    //devuelve la velocidad máxima de la nave sin aplicar modificadores por posición, rebufo, turbo y salud
    {
        get { return VelocityWithWeight * velocityMultiplier; }
    }

    public float VelocityWithWeight //devuelve la velocidad base de la nave afectada por el peso
    {
        get { return Mathf.Clamp(currentVelocity - (GetComponent<Stats>().currentWeight * velocityWeightInfluence),0,Mathf.Infinity); }
    }

    public float AcelerationWithWeight  //devuelve la aceleración de la nave afectada por el peso
    {
        get { return Mathf.Clamp(currentAcceleration - (accelerationWeightInfluence * GetComponent<Stats>().currentWeight),0,Mathf.Infinity); }
    }

}
