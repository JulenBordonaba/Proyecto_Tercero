using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieza : MonoBehaviour {

    public float maxHealth = 100;
    public float Weight = 100;

    private float currentHealth;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.GetKeyDown(KeyCode.A))
  //      {
  //          currentHealth -= 10;
  //      }
  //      if(currentHealth<=0)
  //      {
  //          onPieceDestroyed();
  //      }
	}


    private void onPieceDestroyed()
    {
        gameObject.AddComponent<Rigidbody>();
    }
}
