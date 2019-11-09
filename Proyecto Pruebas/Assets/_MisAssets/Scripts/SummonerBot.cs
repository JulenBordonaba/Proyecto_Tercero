using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerBot : MonoBehaviour
{

    public float velocity;

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="NaveCentre")
        {
            if(other.gameObject.layer!=gameObject.layer)
            {
                animator.enabled = false;
                direction = (other.transform.position - transform.position).normalized;
                inShot = true;
                transform.parent.parent.SetParent(null);
            }
        }
    }
    
}
