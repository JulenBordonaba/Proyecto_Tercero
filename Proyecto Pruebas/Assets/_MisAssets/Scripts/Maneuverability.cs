using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maneuverability : MonoBehaviour
{
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





    public float MaxVelocity    //devuelve la velocidad máxima de la nave sin aplicar modificadores por posición, rebufo, turbo y salud
    {
        get { return VelocityWithWeight * maxVel; }
    }

    public float VelocityWithWeight //devuelve la velocidad base de la nave afectada por el peso
    {
        get { return Velocity() - (Peso() * constanteVelocidadPeso); }
    }

}
