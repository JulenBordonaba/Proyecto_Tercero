using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveManager : MonoBehaviour
{

    [Tooltip("Pon el numero de combustibles correspondiente en Size. Luego elige una de las 4 opciones para cada uno de ellos")]
    public List<TipoCombustible> combustibles;     //tipo del combustible
    public HabilidadCombustible habilidadCombustible { get; set; }  //variable que almacena la habilidad del cumbustible activo
    [Tooltip("Variable que controla si el escudo esta activo o no")]
    public bool inShield = false; //variable de control. Si es true el escudo está activo y no recibe daño
    [Tooltip("Variable que controla si la nave está planeando o no")]
    public bool isPlanning = false;//variable de control. Si es true la nave está planeando


    private int combustibleActivo = 0; //combustible activo, se usa como index para la lista "combustibles"
    private Stats stats;    //variable con las stats de la nave
    private NaveController controller;  //script con el controlador de la nave
    private Maneuverability maneuverability;

    private void Start()
    {
        stats = GetComponent<Stats>();
        controller = GetComponent<NaveController>();
        maneuverability = GetComponent<Maneuverability>();
    }

    private void Update()
    {
        
    }

    private void AsignarCombustibleInicial()
    {
        //asignar combustible que elija el jugador
    }
    private void CombustibleManager()
    {
        //cambiar entre los distintos combustibles
    }

   
    
}
