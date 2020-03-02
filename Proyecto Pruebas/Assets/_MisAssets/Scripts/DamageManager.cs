using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : Photon.PunBehaviour
{
    [Tooltip("Pon el tiempo de inmunidad que tiene la nave despues de recibir daño")]
    public float inmunityCooldown;
    [Tooltip("Pon el daño mínimo para que pueda recibir daño")]
    public float minDamage=-1;
    public DamagedObject damagedObject = DamagedObject.Other;
    private Coroutine fireDamage;

    public byte classId { get; set; }

    public static object Deserialize(byte[] data)
    {
        DamageManager result = new DamageManager();
        result.classId = data[0];
        return result;
    }

    public static byte[] Serialize(object customType)
    {
        DamageManager c = (DamageManager)customType;
        return new byte[] { c.classId };
    }

    private bool canBeDamaged = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartFireDamage(float damage,float loopTime, float duration)
    {
        fireDamage=StartCoroutine(FireDamage(damage, loopTime));
        StartCoroutine(StopFireDamage(duration));
    }

    IEnumerator StopFireDamage(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopCoroutine(fireDamage);
    }

    IEnumerator FireDamage(float damage, float loopTime)
    {
        while(true)
        {
            TakeDamage(damage, true);
            yield return new WaitForSeconds(loopTime);
        }
    }

    public void TakeDamage(float damage, bool weapon)
    {
        //print(gameObject);
        if (minDamage < 0) return;
        if (!canBeDamaged) return;
        if (damage < minDamage && !weapon) return;
        //print("sigue Take Damage");
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

        //print("recived damage: " + damage);

    }

    private IEnumerator InmunityCooldown()
    {
        yield return new WaitForSeconds(inmunityCooldown);
        canBeDamaged = true;
    }

    
}
