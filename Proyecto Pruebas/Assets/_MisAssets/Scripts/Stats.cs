using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Tooltip("Asigna la vida del objeto")]
    public float life;   //vida que tendrá el objeto
    [Tooltip("Asigna la cantidad de daño que va a hacer al chocar")]
    public float collisionDamage;     //daño por colision
    [Tooltip("Asigna la cantidad de daño que va a hacer al disparar. Si no dispara pon 0")]
    public float shotDamage;     //daño por disparo
    [Tooltip("Asigna el peso que tiene el objeto")]
    public float weight;      //pero del objeto. Influye en su daño y velocidad
}
