using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect 
{
    public float duration;


    public void StartEffect()
    {
        DoEffect();
        EffectController.current.StartCoroutine(StopEffect());
    }

    private void DoEffect()
    {

    }

    private void ResetEffect()
    {

    }
    

    private IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(duration);
        ResetEffect();
    }


}
