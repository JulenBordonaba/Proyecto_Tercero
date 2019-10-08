using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pieza : MonoBehaviour {

    /*[Tooltip("Vida representa la vida de la pieza, si llega a 0 la pieza se destruye, Rango:0-100"),Range(0,400)]
    public float vida = 100;
    [Tooltip("Peso es el valor que representa el peso de la pieza, Rango:0-100."), Range(0, 100)]
    public float peso = 100;
    [Tooltip("Velocidad afecta a la velocidad máxima de la nave, Rango:0-100"), Range(0, 100)]
    public float velocidad = 100;
    [Tooltip("Acceleración afecta a la aceleración de la nave, Rango:0-100"), Range(0, 100)]
    public float aceleracion = 100;
    [Tooltip("Maniobrabilidad afecta a el manejo de la nave, lo rápido que gira, Rango:0-100"), Range(0, 100)]
    public float manejo = 100;
    [Tooltip("Daño representa el daño que recive otra nave si colisiona con esta pieza, RAngo:0-100"), Range(0, 100)]
    public float daño = 100;
    [Tooltip("rebufo afecta a la velocidad que ganará la nave cuando este cogiendo rebufo, Rango:0-100"), Range(0, 100)]
    public float rebufo = 100;
    [Tooltip("Turbo afecta a la velocidad que gana la nave durante un turbo, Rango:0-100"), Range(0, 100)]
    public float turbo = 100;
    [Tooltip("Derrape afecta a el valor del derrape de la nave, lo cerrado que es el derrape y la cantidad de energía que gana con él, Rango:0-100."), Range(0, 100)]
    public float derrape = 100;
    [Tooltip("Dash Lateral afecta a la velocidad y distancia a la que la nave hace la carga lateral, Rango:0-100"), Range(0, 100)]
    public float dashLateral = 100;*/
    [Tooltip("Importancia que tiene la pieza, el porcentage en el que aumenta los stats de la nave, Rango:0-100"), Range(0, 100)]
    public float importancia = 25;

    public Slider sliderVida;
    public Text dmgText;
    


    [Tooltip("Esta variable hay que dejarla activada únicamente en la pieza nucleo de la nave")]
    [HideInInspector]
    public float currentHealth;
    public bool nucleo;
    public NaveManager nave { get; set; }

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        
    }


    private void onPieceDestroyed()
    {
        nave.piezas.Remove(this);
        Destroy(gameObject);
    }

    public bool Damage(float ammount)
    {
        return false;
    }
    
    public float Importancia
    {
        get { return importancia / 100; }
    }

    
}
