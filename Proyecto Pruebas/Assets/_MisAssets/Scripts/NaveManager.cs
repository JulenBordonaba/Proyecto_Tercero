using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveManager : MonoBehaviour
{

    [Tooltip("Pon el numero de combustibles correspondiente en Size. Luego elige una de las 4 opciones para cada uno de ellos")]
    public List<TipoCombustible> combustibles;     //tipo del combustible
    public HabilidadCombustible habilidadCombustible { get; set; }
    public bool inShield = false; //variable de control. Si es true el escudo está activo y no recibe daño
    public List<Pieza> piezas = new List<Pieza>(); //lista con todas las piezas de la nave

    private int combustibleActivo = 0; //combustible activo, se usa como index para la lista "combustibles"
    private Transform piezasGameObject;     //transform del objeto que contiene el modelo con todas las piezas
    private bool inDerrape = false;     //variable que indica cuando la nave esta derrapando
    private float position = 0;     //variable que indica la posición de la nave en la carrera, sirve para hacer cálculos de velocidad
    private bool inRebufo = false;  //variable que indica cuando la nave esta cogiendo rebufo, sirve para hacer cálculos de velocidad
    private bool inBoost = false;   //variable que indica cuando la nave esta en un boost, sirve para hacer cálculos de velocidad
    private Pieza nucleo;   //variable que contiene el nucleo de la nave, si este se destruye la nave se destruye
    private Stats stats;    //variable con las stats de la nave

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    private void Update()
    {
        
    }

    private void AsignarCombustibleInicial()
    {

    }
    private void CombustibleManager()
    {

    }

}
