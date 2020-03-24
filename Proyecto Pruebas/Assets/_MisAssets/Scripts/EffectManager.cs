﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Photon.PunBehaviour
{
    public List<DamageManager> damageManagers = new List<DamageManager>();

    public List<EffectData> activeEffects = new List<EffectData>();

    private void Update()
    {
        print(DamageReduction);
    }

    [PunRPC]
    public void StartEffect(EffectData ed)
    {
        if (CheckEffect(ed))
        {
            EffectData _ed = GetEffect(ed.id);
            StopCoroutine( _ed.durationCoroutine);
            _ed.durationCoroutine = null;
            _ed.durationCoroutine = StartCoroutine(EffectDuration(_ed));
            print("resetea tiempo");
        }
        else
        {
            activeEffects.Add(ed);
            ed.dot.dotEffect = StartCoroutine(DOTEffect(ed.dot));
            ed.durationCoroutine = StartCoroutine(EffectDuration(ed));
        }



    }

    EffectData GetEffect(string _id)
    {
        foreach (EffectData ed in activeEffects)
        {
            if (ed.id == _id) return ed;
        }
        return null;
    }

    bool CheckEffect(EffectData _effectData)
    {
        foreach (EffectData ed in activeEffects)
        {
            if (ed.id == _effectData.id) return true;
        }
        return false;
    }

    public void StopEffect(EffectData ed)
    {
        print("Para el efecto");
        if (CheckEffect(ed))
        {
            EffectData _ed = GetEffect(ed.id);
            activeEffects.Remove(_ed);
            StopCoroutine(_ed.dot.dotEffect);
        }
    }

    public void ClearEffects()
    {
        foreach (EffectData ed in activeEffects)
        {
            StopCoroutine(ed.dot.dotEffect);
        }
        activeEffects.Clear();
    }

    IEnumerator EffectDuration(EffectData ed)
    {
        yield return new WaitForSeconds(ed.duration);
        StopEffect(ed);
    }

    IEnumerator DOTEffect(DOT dot)
    {

        while (true)
        {

            print("dot");
            foreach (DamageManager dm in damageManagers)
            {
                if (photonView.isMine)
                {
                    if (dot.damagePerTick > 0)
                    {
                        dm.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, dot.damagePerTick, true);
                    }
                }
            }
            yield return new WaitForSeconds(dot.loopTime);


        }
    }


    #region stats

    private float _DamageReduction()
    {
        float damageReduction = 0;
        foreach (EffectData ed in activeEffects)
        {
            damageReduction += ed.damageReduction;
        }
        return Mathf.Clamp(damageReduction, 0f, 100f);
    }

    public float DamageReduction
    {
        get { return _DamageReduction(); }
    }

    private float _Slow()
    {
        float slow = 0;
        foreach (EffectData ed in activeEffects)
        {
            slow += ed.slow;
        }
        return Mathf.Clamp(slow, 0f, 100f);
    }

    public float Slow
    {
        get { return _Slow(); }
    }

    private float _Velocity()
    {
        float velocity = 0;
        foreach (EffectData ed in activeEffects)
        {
            velocity += ed.velocity;
        }
        return Mathf.Clamp(velocity, 0f, 100f);
    }

    public float Velocity
    {
        get { return _Velocity(); }
    }

    private float _Maneuverability()
    {
        float maneuverability = 0;
        foreach (EffectData ed in activeEffects)
        {
            maneuverability += ed.maneuverability;
        }
        return Mathf.Clamp(maneuverability, 0f, 100f);
    }

    public float Maneuver
    {
        get { return _Maneuverability(); }
    }

    private float _Acceleration()
    {
        float acceleration = 0;
        foreach (EffectData ed in activeEffects)
        {
            acceleration += ed.acceleration;
        }
        return Mathf.Clamp(acceleration, 0f, 100f);
    }

    public float Acceleration
    {
        get { return _Acceleration(); }
    }

    private float _ShotDamage()
    {
        float shotDamage = 0;
        foreach (EffectData ed in activeEffects)
        {
            shotDamage += ed.shotDamage;
        }
        return Mathf.Clamp(shotDamage, 0f, 100f);
    }

    public float ShotDamage
    {
        get { return _ShotDamage(); }
    }

    #endregion

}
