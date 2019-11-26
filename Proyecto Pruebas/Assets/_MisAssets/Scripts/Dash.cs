using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Tooltip("Pon el impulso del dash")]
    public float dashForce;
    [Tooltip("Pon el cooldown del dash")]
    public float cooldown = 0.1f;
    [Tooltip("Pon la duración del dash")]
    public float duration = 0.3f;
    [Tooltip("Pon la fuerza de frenada al acabar el dash, rango 0-1")]
    [Range(0, 1)]
    public float stopForce = 0.2f;

    private bool canDash = true;    //variable que controla cuando puede dashear la nave
    private InputManager inputManager;
    private Animator animator;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        animator= GetComponent<NaveAnimationManager>().animator;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.LeftDash()) UseDash(-1);
        if (inputManager.RightDash()) UseDash(1);
    }

    private void UseDash(int direction)
    {
        if (!canDash) return;
        canDash = false;
        StartCoroutine(Cooldown());

        //dar el impulso
        GetComponent<Rigidbody>().AddForce(GetComponent<NaveController>().modelTransform.right * direction * dashForce, ForceMode.VelocityChange);

        //animación
        switch(direction)
        {
            case 1:
                animator.SetLayerWeight(animator.GetLayerIndex("Dash_Derecha"), 1);
                break;
            case -1:
                animator.SetLayerWeight(animator.GetLayerIndex("Dash_Izquierda"), 1);
                break;
            default:
                break;
        }

        StartCoroutine(EndDash());

    }

    private IEnumerator EndDash()
    {
        yield return new WaitForSeconds(duration);
        animator.SetLayerWeight(animator.GetLayerIndex("Dash_Derecha"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("Dash_Izquierda"), 0);
        for (int i = 0; i < 10; i++)
        {
            Vector3 locVel = GetComponent<NaveController>().modelTransform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
            locVel = new Vector3(locVel.x * (1-stopForce), locVel.y, locVel.z);
            GetComponent<Rigidbody>().velocity = GetComponent<NaveController>().modelTransform.TransformDirection(locVel);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }

}
