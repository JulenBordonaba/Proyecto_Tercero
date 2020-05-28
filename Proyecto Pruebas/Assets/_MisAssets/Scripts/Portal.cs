using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal pair;
    public TipoCombustible tipoCombustible;
    public float fuelRechargeAmmount = 1;
    public Animator animator;
    public AnimationClip activationClip;

    public MeshRenderer mesh;
    public List<NaveManager> naves = new List<NaveManager>();
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
        if (!pair) return;
        if(other.CompareTag("NaveCentre"))
        {
            if (naves.Contains(other.GetComponentInParent<NaveManager>())) return;
            foreach(Combustible c in other.GetComponentsInParent<Combustible>())
            {
                if (c.tipoCombustible == tipoCombustible)
                {
                    c.RechargeFuel(fuelRechargeAmmount);
                }
            }
            NaveManager nm = other.GetComponentInParent<NaveManager>();
            nm.trail.enabled = false;
            nm.gameObject.transform.position = pair.transform.position;
            nm.trail.enabled = true;
            NaveCooldown(nm,1f);
            pair.NaveCooldown(nm, 1f);
        }
    }

    public void NaveCooldown(NaveManager nm, float t)
    {
        naves.Add(nm);
        StartCoroutine(ResetNave(nm,t));
    }

    IEnumerator ResetNave(NaveManager nm, float t)
    {
        yield return new WaitForSeconds(t);
        if(naves.Contains(nm))
        {
            naves.Remove(nm);
        }
    }


}
