using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectManager : Photon.PunBehaviour
{
    public List<DamageManager> damageManagers = new List<DamageManager>();

    public GameObject iconPrefab;

    public GameObject iconContainer;

    public List<EffectData> activeEffects = new List<EffectData>();

    

    private InputManager inputManager;

    public EffectData[] effects;

    private void Start()
    {
        effects = Resources.LoadAll<EffectData>("Data/Effects");
    }

    private void Update()
    {
        foreach(EffectData ed in activeEffects)
        {
            ShowEffectTime(ed);
        }
    }
    
    void ShowEffectTime(EffectData ed)
    {
        if (!ed.effectIcon) return;
        if(ed.permanent)
        {
            ed.effectIcon.durationText.text = "";
            return;
        }
        if(!ed.effectIcon.durationText)
        {
            ed.effectIcon.durationText = ed.effectIcon.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }
        ed.effectIcon.durationText.text = Mathf.FloorToInt(ed.currentDuration).ToString();
    }

    [PunRPC]
    public void StartEffect(string _effect)
    {
        EffectData ed = GetEffectByID(_effect);

        if (CheckEffect(_effect))
        {
            EffectData _ed = GetActiveEffectByID(ed.id);
            StopCoroutine( _ed.durationCoroutine);
            _ed.durationCoroutine = null;
            _ed.durationCoroutine = StartCoroutine(EffectDuration(_ed));
            if(!_ed.effectIcon)
            {
                CreateIcon(_ed);
            }
            print("resetea tiempo");
        }
        else
        {
            print("new effect");
            activeEffects.Add(ed);
            ed.dot.dotEffect = StartCoroutine(DOTEffect(ed.dot));
            ed.durationCoroutine = StartCoroutine(EffectDuration(ed));
            CreateIcon(ed);
        }



    }

    void CreateIcon(EffectData ed)
    {
        EffectIcon newIcon = Instantiate(iconPrefab, iconContainer.transform).GetComponent<EffectIcon>();
        newIcon.icon.sprite = ed.icon;
        ed.effectIcon = newIcon;
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

            //borar icono
            if(_ed.effectIcon!=null)
            {
                Destroy(_ed.effectIcon.gameObject);
                _ed.effectIcon = null;
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

            //borar icono
            if (_ed.effectIcon)
            {
                print("Destruye icono");
                Destroy(_ed.effectIcon.gameObject);
                _ed.effectIcon = null;
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

    

    IEnumerator EffectDuration(EffectData ed)
    {
        if(!ed.permanent)
        {
            ed.currentDuration = ed.duration;
            while(ed.currentDuration>0)
            {
                ed.currentDuration -= Time.deltaTime;
                yield return null;
            }
            StopEffect(ed.id);
        }
        else
        {
            while(true)
            {
                yield return null;
            }
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
