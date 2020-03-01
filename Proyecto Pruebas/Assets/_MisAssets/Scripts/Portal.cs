using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal pair;
    public TipoCombustible tipoCombustible;
    public float fuelRechargeAmmount = 1;

    public MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaterial(Material m)
    {
        mesh.material = m;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("NaveCentre"))
        {
            foreach(Combustible c in other.GetComponentsInParent<Combustible>())
            {
                if (c.tipoCombustible == tipoCombustible)
                {
                    c.RechargeFuel(fuelRechargeAmmount);
                }
            }
            other.GetComponentInParent<NaveManager>().gameObject.transform.position = pair.transform.position;
        }
    }
}
