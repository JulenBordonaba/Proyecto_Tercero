using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveAnimationManager : MonoBehaviour
{
    [Tooltip("Pon el animator de la nave")]
    public Animator animator;
    [Header("Booleanas animator")]
    public bool move = false;
    public bool plane = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Move", move);
        animator.SetBool("Plane", plane);
    }
}
