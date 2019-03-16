using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieza : MonoBehaviour {
    
    [Tooltip("Health value between 0 and 300."),Range(0,300)]
    public float health = 100;
    [Tooltip("Weight value between 0 and 100."), Range(0, 100)]
    public float weight = 100;
    [Tooltip("Maximum speed value between 0 and 100."), Range(0, 100)]
    public float maxVel = 100;
    [Tooltip("Aceeleration value between 0 and 100."), Range(0, 100)]
    public float acceleration = 100;
    [Tooltip("Manoeuvrability value between 0 and 100."), Range(0, 100)]
    public float manoeuvrability = 100;
    [Tooltip("Damage value between 0 and 100."), Range(0, 100)]
    public float damage = 100;
    [Tooltip("Recoil value between 0 and 100."), Range(0, 100)]
    public float recoil = 100;
    [Tooltip("Turbo value between 0 and 100."), Range(0, 100)]
    public float turbo = 100;
    [Tooltip("Skid value between 0 and 100."), Range(0, 100)]
    public float skid = 100;
    [Tooltip("SideDash value between 0 and 100."), Range(0, 100)]
    public float sideDash = 100;
    public bool skill;



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
