using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieza : MonoBehaviour {
    
    [Tooltip("Health representa la vida de la pieza, si llega a 0 la pieza se destruye, Rango:0-300"),Range(0,300)]
    public float health = 100;
    [Tooltip("Weight es el valor que representa el peso de la pieza, Rango:0-100."), Range(0, 100)]
    public float weight = 100;
    [Tooltip("Velocity afecta a la velocidad máxima de la nave, Rango:0-100"), Range(0, 100)]
    public float velocity = 100;
    [Tooltip("Acceleration afecta a la aceleración de la nave, Rango:0-100"), Range(0, 100)]
    public float acceleration = 100;
    [Tooltip("Maniobrabilidad afecta a el manejo de la nave, lo rápido que gira, Rango:0-100"), Range(0, 100)]
    public float maniobrabilidad = 100;
    [Tooltip("Damage representa el daño que recive otra nave si colisiona con esta pieza, RAngo:0-100"), Range(0, 100)]
    public float damage = 100;
    [Tooltip("rebufo afecta a la velocidad que ganará la nave cuando este cogiendo rebufo, Rango:0-100"), Range(0, 100)]
    public float rebufo = 100;
    [Tooltip("Turbo afecta a la velocidad que gana la nave durante un turbo, Rango:0-100"), Range(0, 100)]
    public float turbo = 100;
    [Tooltip("Derrape afecta a el valor del derrape de la nave, lo cerrado que es el derrape y la cantidad de energía que gana con él, Rango:0-100."), Range(0, 100)]
    public float derrape = 100;
    [Tooltip("Dash Lateral afecta a la velocidad y distancia a la que la nave hace la carga lateral, Rango:0-100"), Range(0, 100)]
    public float dashLateral = 100;


    [Tooltip("Esta variable hay que dejarla activada únicamente en la pieza nucleo de la nave")]
    public bool nucleo;

    private float currentHealth;
    public Nave nave { get; set; }

	// Use this for initialization
	void Start () {
        currentHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
	}


    private void onPieceDestroyed()
    {
        nave.piezas.Remove(this);
        Destroy(gameObject);
    }

    public void Damage(float ammount)
    {
        currentHealth -= ammount;
        if(currentHealth<=0)
        {
            onPieceDestroyed();
        }
    }

    public float CalculateCollisionDamage(float collisionForce)
    {
        return collisionForce;
    }
}
