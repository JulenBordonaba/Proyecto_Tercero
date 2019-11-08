using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveManager : MonoBehaviour
{

    [Tooltip("Pon el numero de combustibles correspondiente en Size. Luego elige una de las 4 opciones para cada uno de ellos")]
    public List<TipoCombustible> combustibles;     //tipo del combustible
    [Tooltip("Habilidad de combustible activa")]
    public HabilidadCombustible habilidadCombustible; //variable que almacena la habilidad del cumbustible activo
    [Tooltip("Variable que controla si la nave está planeando o no")]
    public bool isPlanning = false;//variable de control. Si es true la nave está planeando
    [Tooltip("Pon la reducción de daño por colisión")]
    [Range(0, 1)]
    public float collisionDamageReduction = 0.8f;
    [Tooltip("Pon el prefab de la explosión")]
    public GameObject explosionPrefab;


    public int combustibleActivo = 0; //combustible activo, se usa como index para la lista "combustibles"
    private Stats stats;    //variable con las stats de la nave
    private NaveController controller;  //script con el controlador de la nave
    private Maneuverability maneuverability;

    private void Start()
    {
        stats = GetComponent<Stats>();
        controller = GetComponent<NaveController>();
        maneuverability = GetComponent<Maneuverability>();
        AsignarCombustibleInicial();
    }

    private void Update()
    {
        FuelManager();
    }

    private void AsignarCombustibleInicial()
    {
        //asignar el combustible que elija el jugador
        try
        {
            //habrá que cambiarlo para poner el combustible que elija el jugador
            habilidadCombustible = GetComponent(combustibles[0].ToString()) as HabilidadCombustible;
            combustibleActivo = 0;  
        }
        catch
        {
            throw new Exception("Fallo al cargar habilidad de combustible");
        }
    }
    


    private void FuelManager()
    {
        //cambiar entre los distintos combustibles
        if (InputManager.ChangeFuelLeft())
        {
            try
            {
                combustibleActivo -= 1;
                if (combustibleActivo < 0) //comprueba que no se salga del límite del array
                {
                    combustibleActivo = combustibles.Count - 1;
                }
                habilidadCombustible = GetComponent(combustibles[combustibleActivo].ToString()) as HabilidadCombustible;


            }
            catch
            {
                throw new Exception("Fallo al cambiar habilidad de combustible");
            }
        }
        else if (InputManager.ChangeFuelRight())
        {
            try
            {
                combustibleActivo += 1;
                if (combustibleActivo >= combustibles.Count) //comprueba que no se salga del límite del array
                {
                    combustibleActivo = 0;
                }
                habilidadCombustible = GetComponent(combustibles[combustibleActivo].ToString()) as HabilidadCombustible;


            }
            catch
            {
                throw new Exception("Fallo al cambiar habilidad de combustible");
            }
        }
        if (InputManager.UseFuel())
        {
            habilidadCombustible.Use();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Obstacle")
        {
            DamageManager dm = collision.contacts[0].thisCollider.gameObject.GetComponentInParent<DamageManager>();
            float impactForce = Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity);
            impactForce = Mathf.Clamp(impactForce, 0, float.MaxValue);
            if(collision.contacts[0].thisCollider.gameObject.GetComponentInParent<DamageManager>())
            {
                collision.contacts[0].thisCollider.gameObject.GetComponentInParent<DamageManager>().TakeDamage(impactForce * GetComponent<Stats>().currentCollisionDamage * (1 - collisionDamageReduction));
            }
            if (collision.gameObject.GetComponent<DamageManager>())
            {
                collision.gameObject.GetComponent<DamageManager>().TakeDamage(impactForce * collision.contacts[0].thisCollider.gameObject.GetComponentInParent<Stats>().currentCollisionDamage * (1 - collisionDamageReduction));
            }
        }
        
    }

    public void OnShipDestroyed()
    {
        GameObject explosion = Instantiate(explosionPrefab, GetComponent<NaveController>().transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponentInChildren<ParticleSystem>().main.duration);
        Destroy(transform.parent.gameObject);
    }



    private float CalculateImpactForce(Vector3 collisionNormal, Vector3 collisionVelocity)
    {
        return Vector3.Dot(collisionNormal, collisionVelocity);
    }

}
