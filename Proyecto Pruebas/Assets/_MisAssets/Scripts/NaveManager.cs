using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveManager : MonoBehaviour
{

    [Tooltip("Pon el numero de combustibles correspondiente en Size. Luego elige una de las 4 opciones para cada uno de ellos")]
    public List<TipoCombustible> combustibles;     //tipo del combustible
    public HabilidadCombustible habilidadCombustible { get; set; }  //variable que almacena la habilidad del cumbustible activo
    public bool inShield = false; //variable de control. Si es true el escudo está activo y no recibe daño
    public List<Pieza> piezas = new List<Pieza>(); //lista con todas las piezas de la nave

    private int combustibleActivo = 0; //combustible activo, se usa como index para la lista "combustibles"
    private Transform model;     //transform del objeto que contiene el modelo con todas las piezas
    private bool inDerrape = false;     //variable que indica cuando la nave esta derrapando
    private float position = 0;     //variable que indica la posición de la nave en la carrera, sirve para hacer cálculos de velocidad
    private bool inRebufo = false;  //variable que indica cuando la nave esta cogiendo rebufo, sirve para hacer cálculos de velocidad
    private bool inBoost = false;   //variable que indica cuando la nave esta en un boost, sirve para hacer cálculos de velocidad
    private Pieza nucleo;   //variable que contiene el nucleo de la nave, si este se destruye la nave se destruye
    private Stats stats;    //variable con las stats de la nave
    private NaveController controller;  //script con el controlador de la nave
    private Maneuverability maneuverability;

    private void Start()
    {
        stats = GetComponent<Stats>();
        controller = GetComponent<NaveController>();
        maneuverability = GetComponent<Maneuverability>();
        

        piezas = new List<Pieza>(GetComponentsInChildren<Pieza>());
        model = piezas[0].transform.parent;

        foreach (Pieza p in piezas)
        {
            //p.nave = this;
            if (p.nucleo)
            {
                nucleo = p;
            }
        }
    }

    private void Update()
    {
        controller.Controller();
    }

    private void AsignarCombustibleInicial()
    {
        //asignar combustible que elija el jugador
    }
    private void CombustibleManager()
    {
        //cambiar entre los distintos combustibles
    }

   
    public bool AnyMovementKeys
    {
        get { return (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Joystick1Button2) || Input.GetAxis("Nave Vertical") != 0)/* || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)*/; }
    }

    public float VelocityFormula    //devuelve la velocidad máxima de la nave aplicando todos los modificadores
    {
        get { return GetComponent<Maneuverability>().MaxVelocity + (PorcentajeSalud * healthConst) + (DistanciaPrimero * positionConst) + ((rebufoConst * (inRebufo ? 1 : 0)) * Rebufo()) + ((boostConst * (inBoost ? 1 : 0)) * Turbo()); }
    }

    public float PorcentajeSalud    //devuelve el porcentaje de salud de la nave
    {
        get { return (nucleo.currentHealth / GetComponent<Stats>().life) * 100; }
    }

    public float DistanciaPrimero   //devuelve la distancia de la nave respecto al primero de la carrera
    {
        get { return 1; }
    }
}
