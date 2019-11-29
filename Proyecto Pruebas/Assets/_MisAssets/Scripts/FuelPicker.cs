using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPicker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.gameObject.GetComponent<CombustibleObject>())
        {
            switch (other.gameObject.GetComponent<CombustibleObject>().tipo)
            {
                case TipoCombustible.Escudo:
                    GetComponentInParent<Escudo>().combustible.currentAmmount += other.gameObject.GetComponent<CombustibleObject>().recharge;
                    GetComponentInParent<Escudo>().combustible.currentAmmount = Mathf.Clamp(GetComponentInParent<Escudo>().combustible.currentAmmount, 0, GetComponentInParent<Escudo>().combustible.deposit);
                    break;
                case TipoCombustible.Turbo:
                    GetComponentInParent<Turbo>().combustible.currentAmmount += other.gameObject.GetComponent<CombustibleObject>().recharge;
                    GetComponentInParent<Turbo>().combustible.currentAmmount = Mathf.Clamp(GetComponentInParent<Turbo>().combustible.currentAmmount, 0, GetComponentInParent<Turbo>().combustible.deposit);
                    break;
                case TipoCombustible.Salto:
                    GetComponentInParent<Salto>().combustible.currentAmmount += other.gameObject.GetComponent<CombustibleObject>().recharge;
                    GetComponentInParent<Salto>().combustible.currentAmmount = Mathf.Clamp(GetComponentInParent<Salto>().combustible.currentAmmount, 0, GetComponentInParent<Salto>().combustible.deposit);
                    break;
                case TipoCombustible.Reparar:
                    GetComponentInParent<Reparar>().combustible.currentAmmount += other.gameObject.GetComponent<CombustibleObject>().recharge;
                    GetComponentInParent<Reparar>().combustible.currentAmmount = Mathf.Clamp(GetComponentInParent<Reparar>().combustible.currentAmmount, 0, GetComponentInParent<Reparar>().combustible.deposit);
                    break;
            }
            Destroy(other.gameObject.gameObject);
        }
    }
}
