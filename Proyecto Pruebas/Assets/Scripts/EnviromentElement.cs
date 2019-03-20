using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentElement : MonoBehaviour
{
    public float damage;
    public float vida;
    public float peso;

    public bool destruible;

    private float currentHealth;

    private void Start()
    {
        currentHealth = vida;
    }

    public void Damage(float ammount)
    {
        currentHealth -= ammount;
        print(ammount);
    }
}
