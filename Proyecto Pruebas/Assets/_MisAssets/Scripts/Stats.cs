using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Atributos totales")]
    [Tooltip("Asigna la vida del objeto")]
    public float life;   //vida que tendrá el objeto
    [Tooltip("Asigna la cantidad de daño que va a hacer al chocar")]
    public float collisionDamage;     //daño por colision
    [Tooltip("Asigna la cantidad de daño que va a hacer al disparar. Si no dispara pon 0")]
    public float shotDamage;     //daño por disparo
    [Tooltip("Asigna el peso que tiene el objeto")]
    public float weight;      //pero del objeto. Influye en su daño y velocidad

    public float currentLife;     //vida que tendrá el objeto
    public float currentCollisionDamage;        //daño por colision
    public float currentShotDamage;       //daño por disparo
    public float currentWeight;         //pero del objeto. Influye en su daño y velocidad


    public byte classId { get; set; }

    private void Awake()
    {
        //al inicializarse el objeto los valores actuales son iguales al doble de los totales
        currentLife = life;
        currentCollisionDamage = collisionDamage;
        currentShotDamage = shotDamage;
        currentWeight = weight;
    }

    public static object Deserialize(byte[] data)
    {
        Stats result = new Stats();
        result.classId = data[0];
        return result;
    }

    public static byte[] Serialize(object customType)
    {
        Stats c = (Stats)customType;
        return new byte[] { c.classId };
    }


    public void AddPieceValues(float importance)
    {
        currentLife += life * (importance / 100);
        currentCollisionDamage += collisionDamage * (importance / 100);
        currentShotDamage += shotDamage * (importance / 100);
        currentWeight += weight * (importance / 100);
    }

    public void OnPieceDestroyed(float importance)
    {
        currentLife -= life * (importance / 100);
        currentCollisionDamage -= collisionDamage * (importance / 100);
        currentShotDamage -= shotDamage * (importance / 100);
        currentWeight -= weight * (importance / 100);
    }


}
