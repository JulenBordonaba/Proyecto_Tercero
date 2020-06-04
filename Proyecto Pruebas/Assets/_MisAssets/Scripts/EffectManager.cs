using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectManager : Photon.PunBehaviour
{
    public List<DamageManager> damageManagers = new List<DamageManager>();

    public GameObject iconPrefab;

    public AudioSource effectAudioSource;

    public GameObject iconContainer;

    public List<EffectData> activeEffects = new List<EffectData>();

    public List<EffectIcon> activeIcons = new List<EffectIcon>();

    private InputManager inputManager;

    public EffectData[] effects;

    private void Start()
    {
        effects = Resources.LoadAll<EffectData>("Data/Effects");
    }
    
    

    [PunRPC]
    public void StartEffect(string _effect)
    {
        EffectData ed = GetEffectByID(_effect);

        if (CheckEffect(_effect))
        {
            EffectData _ed = GetActiveEffectByID(ed.id);

            EffectIcon icon = GetActiveIconByID(_ed.id);

            if (!icon)
            {
                icon=CreateIcon(_ed);
            }
            else
            {
                StopCoroutine(icon.durationCoroutine);
                icon.durationCoroutine = null;
            }

            icon.durationCoroutine = StartCoroutine(EffectDuration(icon));
            if(_ed.effectClip)
            {
                effectAudioSource.PlayOneShot(_ed.effectClip);
            }
            print("resetea tiempo");
        }
        else
        {
            print("new effect");
            activeEffects.Add(ed);
            ed.dot.dotEffect = StartCoroutine(DOTEffect(ed.dot));
            EffectIcon icon = CreateIcon(ed);
            icon.durationCoroutine = StartCoroutine(EffectDuration(icon));
            if (ed.effectClip)
            {
                effectAudioSource.PlayOneShot(ed.effectClip);
            }
        }



    }

    EffectIcon CreateIcon(EffectData ed)
    {
        EffectIcon newIcon = Instantiate(iconPrefab, iconContainer.transform).GetComponent<EffectIcon>();
        newIcon.icon.sprite = ed.icon;
        newIcon.effect = ed;
        activeIcons.Add(newIcon);
        return newIcon;
    }

    EffectIcon GetActiveIconByID(string _id)
    {
        foreach (EffectIcon ei in activeIcons)
        {
            if (ei.effect.id == _id) return ei;
        }
        return null;
    }

    EffectData GetEffectByID(string _id)
    {
        foreach (EffectData ed in effects)
        {
            if (ed.id == _id) return ed;
        }
        return null;
    }

    EffectData GetActiveEffectByID(string _id)
    {
        foreach (EffectData ed in activeEffects)
        {
            if (ed.id == _id) return ed;
        }
        return null;
    }

    bool CheckEffect(string _effectDataID)
    {
        foreach (EffectData ed in activeEffects)
        {
            if (ed.id == _effectDataID) return true;
        }
        return false;
    }

    [PunRPC]
    public void StopEffect(string ed)
    {
        print("Para el efecto");
        if (CheckEffect(ed))
        {
            EffectData _ed = GetActiveEffectByID(ed);

            //si es permanente no se puede borrar
            if (_ed.permanent) return;

            //se quita el efecto de los efectos activos
            activeEffects.Remove(_ed);

            EffectIcon icon = GetActiveIconByID(_ed.id);

            //borar icono
            if (icon)
            {
                activeIcons.Remove(icon);
                Destroy(icon.gameObject);
            }

            //se para el dot en caso de que lo haya
            if(_ed.dot.dotEffect!=null)
            {
                StopCoroutine(_ed.dot.dotEffect);
            }
        }
    }

    [PunRPC]
    public void StopPermanentEffect(string ed)
    {
        print("Para el efecto");
        if (CheckEffect(ed))
        {
            EffectData _ed = GetActiveEffectByID(ed);
            
            //se quita el efecto de los efectos activos
            activeEffects.Remove(_ed);
            
            EffectIcon icon = GetActiveIconByID(_ed.id);

            //borar icono
            if (icon)
            {
                activeIcons.Remove(icon);
                Destroy(icon.gameObject);
            }

            //se para el dot en caso de que lo haya
            if (_ed.dot.dotEffect != null)
            {
                StopCoroutine(_ed.dot.dotEffect);
            }
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

    

    IEnumerator EffectDuration(EffectIcon icon)
    {
        if(!icon.effect.permanent)
        {
            icon.currentDuration = icon.effect.duration;
            while(icon.currentDuration>0)
            {
                icon.currentDuration -= Time.deltaTime;
                yield return null;
            }
            StopEffect(icon.effect.id);
        }
        else
        {
            icon.currentDuration = Mathf.Infinity;
            while (icon.currentDuration > 0)
            {
                icon.currentDuration -= Time.deltaTime;
                yield return null;
            }
            StopEffect(icon.effect.id);
        }
    }

    IEnumerator DOTEffect(DOT dot)
    {

        while (true)
        {

            //print("dot");
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

    #region SilenceFuels
    private bool _SilenceFuels()
    {
        foreach (EffectData ed in activeEffects)
        {
            if (ed.silenceFuels) return true;
        }
        return false;
    }

    public bool SilenceFuels
    {
        get { return _SilenceFuels(); }
    }
    #endregion

    #region SilenceAbilities
    private bool _SilenceAbilities()
    {
        foreach (EffectData ed in activeEffects)
        {
            if (ed.silenceAbilities) return true;
        }
        return false;
    }

    public bool SilenceAbilities
    {
        get { return _SilenceAbilities(); }
    }
    #endregion

    #region InvertControls
    private bool _InvertControls()
    {
        foreach (EffectData ed in activeEffects)
        {
            if (ed.invertControls) return true;
        }
        return false;
    }

    public bool InvertControls
    {
        get { return _InvertControls(); }
    }
    #endregion

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
