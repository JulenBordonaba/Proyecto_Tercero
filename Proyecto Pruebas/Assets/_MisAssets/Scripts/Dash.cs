using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Tooltip("Pon el impulso del dash")]
    public float dashForce;
    [Tooltip("Pon el cooldown del dash")]
    public float cooldown = 0.1f;

    private bool canDash = true;    //variable que controla cuando puede dashear la nave

    
    // Update is called once per frame
    void Update()
    {
        if (InputManager.LeftDash()) UseDash(-1);
        if (InputManager.RightDash()) UseDash(1);
    }

    private void UseDash(int direction)
    {
        if (!canDash) return;
        canDash = false;
        StartCoroutine(Cooldown());

        //dar el impulso
        GetComponent<Rigidbody>().AddForce(GetComponent<NaveController>().modelTransform.right * direction * dashForce, ForceMode.VelocityChange);

    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }

}
