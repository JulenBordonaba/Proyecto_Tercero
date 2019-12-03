using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [Tooltip("Pon el tiempo de inmunidad que tiene la nave despues de recibir daño")]
    public float inmunityCooldown;
    [Tooltip("Pon el daño mínimo para que pueda recibir daño")]
    public float minDamage=-1;

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
        if (minDamage < 0) return;
        if (!canBeDamaged) return;
        if (damage < minDamage && !weapon) return;
        //recibir daño
        if(GetComponent<Stats>())
        {
            if(inmunityCooldown>0)
            {
                canBeDamaged = false;
                StartCoroutine(InmunityCooldown());
            }
            GetComponent<Stats>().currentLife -= damage;
            if (GetComponent<Stats>().currentLife <= 0) Destroy(gameObject);
        }
        else if(GetComponentInParent<Pieza>())
        {
            if(inmunityCooldown>0)
            {
                canBeDamaged = false;
                StartCoroutine(InmunityCooldown());
            }
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
