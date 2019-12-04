using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveManager : MonoBehaviour
{
    //public static Combustible combustible;

    [Tooltip("Pon el numero de combustibles correspondiente en Size. Luego elige una de las 4 opciones para cada uno de ellos")]
    public List<TipoCombustible> combustibles;     //tipo del combustible
    [Tooltip("Habilidad de combustible activa")]
    public HabilidadCombustible habilidadCombustible; //variable que almacena la habilidad del cumbustible activo
    [Tooltip("Variable que controla si la nave está planeando o no")]
    public bool isPlanning = false;//variable de control. Si es true la nave está planeando
    [Tooltip("Pon la reducción de daño por colisión")]
    public float collisionDamageReduction = 0.8f;
    [Tooltip("Pon el prefab de la explosión")]
    public GameObject explosionPrefab;
    [Tooltip("Pon el tiempo que se queda la cámaras antes de mostrar el game over")]
    public float deathTime = 5;
    [Tooltip("Pon la cámara de la nave")]
    public Camera myCamera;
    public TrailRenderer trail;
    public Combustible combustible;
    public GameObject victoryImage;
    


    public int combustibleActivo = 0; //combustible activo, se usa como index para la lista "combustibles"
    private Stats stats;    //variable con las stats de la nave
    private NaveController controller;  //script con el controlador de la nave
    private Maneuverability maneuverability;
    private InputManager inputManager;
    private Rigidbody rb;

    private bool fuelInLeft = false;
    private bool fuelInRight = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager.navesList.Add(this);
        inputManager = GetComponent<InputManager>();
        stats = GetComponent<Stats>();
        controller = GetComponent<NaveController>();
        maneuverability = GetComponent<Maneuverability>();
        AsignarCombustibleInicial();
    }

    private void Update()
    {
        FuelManager();
        combustible.PasiveConsumption();
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
        if (PauseManager.inPause) return;

        combustible = habilidadCombustible.combustible;
        Vector3 locVel = GetComponent<NaveController>().modelTransform.InverseTransformDirection(rb.velocity);
        if (locVel.z<=2)
        {
            trail.enabled = false;
        }
        else
        {
            trail.enabled = true;
        }

        if (trail.enabled)
        {
            trail.material.color = habilidadCombustible.combustible.color;
        }

        Direction fuelSide = ChangeFuelManager();
        //cambiar entre los distintos combustibles
        if (fuelSide == Direction.Left)
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
        if (fuelSide == Direction.Right || combustible.currentAmmount<=0)
        {
            //print("entra en right");
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
        if (inputManager.UseFuel())
        {
            habilidadCombustible.Use();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag=="Nave")
        {
            DamageManager dm = collision.contacts[0].thisCollider.gameObject.GetComponentInParent<DamageManager>();
            float impactForce = Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity);
            impactForce = Mathf.Clamp(impactForce, 0, float.MaxValue);
            if (collision.contacts[0].thisCollider.gameObject.GetComponentInParent<DamageManager>())
            {
                collision.contacts[0].thisCollider.gameObject.GetComponentInParent<DamageManager>().TakeDamage(impactForce * GetComponent<Stats>().currentCollisionDamage * (1 / collisionDamageReduction),false);
            }
            if (collision.gameObject.GetComponent<DamageManager>())
            {
                collision.gameObject.GetComponent<DamageManager>().TakeDamage(impactForce * collision.contacts[0].thisCollider.gameObject.GetComponentInParent<Stats>().currentCollisionDamage * (1 / collisionDamageReduction), false);
            }
            else if (collision.gameObject.GetComponentInParent<DamageManager>())
            {
                collision.gameObject.GetComponentInParent<DamageManager>().TakeDamage(impactForce * collision.contacts[0].thisCollider.gameObject.GetComponentInParent<Stats>().currentCollisionDamage * (1 / collisionDamageReduction), false);
            }
        }
        

    }

    

    public void SetWeaponObjectives(LayerMask layers)
    {

    }

    private Direction ChangeFuelManager()
    {
        if (inputManager.ChangeFuel() > 0)
        {
            if (!fuelInRight)
            {
                fuelInLeft = false;
                fuelInRight = true;
                return Direction.Right;
            }
        }
        if (inputManager.ChangeFuel() < 0)
        {
            if (!fuelInLeft)
            {
                fuelInRight = false;
                fuelInLeft = true;
                return Direction.Left;
            }
        }
        if(inputManager.ChangeFuel() == 0)
        {
            fuelInRight = false;
            fuelInLeft = false;
            return Direction.None;
        }

        return Direction.None;
    }

    public void OnShipDestroyed()
    {
        StartCoroutine(OnShipDestroyedCoroutine());
    }

    public IEnumerator OnShipDestroyedCoroutine()
    {
        yield return new WaitForEndOfFrame();
        myCamera.GetComponent<CameraController>().naveDestruida = true;
        myCamera.gameObject.GetComponent<CameraController>().OnShipDestroyed(deathTime);
        GameManager.navesList.Remove(this);
        GameObject explosion = Instantiate(explosionPrefab, GetComponent<NaveController>().transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponentInChildren<ParticleSystem>().main.duration);
        Destroy(transform.parent.gameObject);
    }
    



    private float CalculateImpactForce(Vector3 collisionNormal, Vector3 collisionVelocity)
    {
        return Vector3.Dot(collisionNormal, collisionVelocity);
    }

}
