using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerBot : MonoBehaviour
{
    [Tooltip("Pon la velocidad a la que saldrá el dron disparado")]
    public float velocity;
    [Tooltip("Pon las layers de los objetos que el dron debe tomar como objetivo")]
    public LayerMask hitLayers;

    private Animator animator;  //animator del bot
    private Vector3 direction;
    private bool inShot = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inShot) return;
        Move();

    }

    private void Move()
    {
        transform.parent.Translate(direction * velocity * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (inShot) return;
        if(other.gameObject.tag=="NaveCentre")
        {
            if(other.gameObject.layer!=gameObject.layer)
            {
                direction = (other.transform.position - transform.position).normalized;
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = direction;
                RaycastHit hit;


                if (Physics.Raycast(ray,out hit))
                {
                    if (((1 << hit.transform.gameObject.layer) & hitLayers) != 0)
                    {
                        animator.enabled = false;
                        inShot = true;
                        transform.parent.parent.SetParent(null);
                        GetComponentInParent<SummonerBotCollision>().inShot = true;
                    }
                }
            }
        }
    }
    
}
