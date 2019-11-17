using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [Tooltip("Pon el tiempo de inmunidad que tiene la nave despues de recibir daño")]
    public float inmunityCooldown;
    [Tooltip("Pon el daño mínimo para que pueda recibir daño")]
    public float minDamage;

    private bool canBeDamaged = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage, bool weapon)
    {
        print("TakeDamage1");
        if (!canBeDamaged) return;
        if (damage < minDamage && !weapon) return;
        print("TakeDamage2");
        //recibir daño
        if(GetComponent<Stats>())
        {
            canBeDamaged = false;
            StartCoroutine(InmunityCooldown());
            GetComponent<Stats>().currentLife -= damage;
            if (GetComponent<Stats>().currentLife <= 0) Destroy(gameObject);
        }
        else if(GetComponentInParent<Pieza>())
        {
            canBeDamaged = false;
            StartCoroutine(InmunityCooldown());
            GetComponentInParent<Pieza>().Damage(damage);
        }

        print(damage);

    }

    private IEnumerator InmunityCooldown()
    {
        yield return new WaitForSeconds(inmunityCooldown);
        canBeDamaged = true;
    }

    
}
