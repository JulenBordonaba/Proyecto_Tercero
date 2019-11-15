using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerBot : MonoBehaviour
{
    [Tooltip("Pon la velocidad a la que saldrá el dron disparado")]
    public float velocity;
    [Tooltip("Pon las layers de los objetos que el dron debe tomar como objetivo")]
    public LayerMask hitLayers;
    [Tooltip("Pon la corrección de la trayectoria del bot")]
    public float trayectoryCorrection = 1;
    [Tooltip("Pon el aumento de velocidad del bot respecto al tiempo")]
    public float velocityIncrease = 10;
    //[Tooltip("Pon el aumento de la corrección de trayectoria por segundo")]
    //public float correctionIncrease = 50;

    private Animator animator;  //animator del bot
    private Vector3 direction;
    private bool inShot = false;
    private Transform objective;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!inShot) return;
        IncreaseVelocity();
        //IncreaseCorrection();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inShot) return;
        Redirect();
        Move();
    }

    //private void IncreaseCorrection()
    //{
    //    trayectoryCorrection += correctionIncrease * Time.deltaTime;
    //}

    private void IncreaseVelocity()
    {
        velocity += velocityIncrease * Time.deltaTime;
    }

    private void Redirect()
    {
        direction = Vector3.Lerp(direction, (objective.position - transform.position).normalized, Time.deltaTime * trayectoryCorrection).normalized;
        Debug.DrawRay(transform.position, direction * 1000, Color.blue);
    }

    private void Move()
    {
        transform.parent.Translate(direction * velocity * Time.fixedDeltaTime, Space.World);
    }

    private void OnTriggerStay(Collider other)
    {
        if (inShot) return;
        if(other.gameObject.tag=="NaveCentre")
        {
            if(other.gameObject.layer!=gameObject.layer && other.gameObject.layer!=GetComponentInParent<NaveController>().gameObject.layer)
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
                        direction = (other.transform.position - transform.position).normalized;
                        animator.enabled = false;
                        inShot = true;
                        objective = other.transform;
                        transform.parent.parent.SetParent(null);
                        GetComponentInParent<SummonerBotCollision>().inShot = true;
                        //Time.timeScale = 0.1f;
                    }
                }
            }
        }
    }
    
}
