using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieza : MonoBehaviour {

    public float maxHealth = 100;
    public float Weight = 100;

    public bool nucleo;

    private float currentHealth;
    public Nave nave { get; set; }

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
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
