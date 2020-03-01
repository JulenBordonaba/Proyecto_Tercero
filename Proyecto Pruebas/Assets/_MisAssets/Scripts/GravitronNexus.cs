using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitronNexus : MonoBehaviour
{
    public float effectRadius = 30f;
    public float force = 10f;
    public float damage = 100f;
    public float cooldown = 10f;

    private MeshRenderer mesh;
    private bool isActive = true;
    private Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        colliders = GetComponents<Collider>();
        SetRadius();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetRadius()
    {
        foreach (SphereCollider sc in GetComponents<SphereCollider>())
        {
            if (sc.isTrigger)
            {
                sc.radius = effectRadius;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;
        if(other.CompareTag("NaveCentre"))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            rb.AddForce((transform.position - other.transform.position) * force / Vector3.Distance(other.transform.position, transform.position),ForceMode.Impulse);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        print("entra a OnCollisionEnter" + collision.gameObject.name);
        if (collision.gameObject.GetComponentInParent<NaveManager>())
        {
            foreach(DamageManager dm in collision.gameObject.GetComponentsInChildren<DamageManager>())
            dm.TakeDamage(damage, true);
            StartCoroutine(DeactivateNexus());
        }
    }

    void SetActive(bool b)
    {
        mesh.enabled = b;
        isActive = b;
        foreach (Collider c in colliders)
        {
            c.enabled = b;
        }
    }

    public IEnumerator DeactivateNexus()
    {
        SetActive(false);
        yield return new WaitForSeconds(cooldown);
        SetActive(true);
    }

}
