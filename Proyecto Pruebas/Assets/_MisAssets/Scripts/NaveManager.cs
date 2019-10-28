using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveManager : MonoBehaviour
{

    [Tooltip("Pon el numero de combustibles correspondiente en Size. Luego elige una de las 4 opciones para cada uno de ellos")]
    public List<TipoCombustible> combustibles;     //tipo del combustible
    public HabilidadCombustible habilidadCombustible; //variable que almacena la habilidad del cumbustible activo
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
        AsignarCombustibleInicial();
    }

    private void Update()
    {
        CombustibleManager();
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
    private void CombustibleManager()
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
                Debug.Log(combustibles.Count);
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
        else if (InputManager.UseFuel())
        {
            habilidadCombustible.Use();
        }
    }

    

}
