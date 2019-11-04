using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [Tooltip("Pon el tiempo de inmunidad que tiene la nave despues de recibir daño")]
    public float inmunityCooldown;

    private bool canBeDamaged = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (!canBeDamaged) return;
        canBeDamaged = false;
        StartCoroutine(InmunityCooldown());

        //recibir daño
        if(GetComponent<Stats>())
        {
            GetComponent<Stats>().currentLife -= damage;
            if (GetComponent<Stats>().currentLife <= 0) Destroy(gameObject);
        }
        else if(GetComponent<Pieza>())
        {
            GetComponent<Pieza>().Damage(damage);
        }

        print(damage);

    }

    private IEnumerator InmunityCooldown()
    {
        yield return new WaitForSeconds(inmunityCooldown);
        canBeDamaged = true;
    }

    
}
