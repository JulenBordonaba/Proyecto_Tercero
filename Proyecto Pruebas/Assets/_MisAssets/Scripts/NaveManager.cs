using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NaveManager : Photon.PunBehaviour
{
    //public static Combustible combustible;

    [Tooltip("Pon el numero de combustibles correspondiente en Size. Luego elige una de las 4 opciones para cada uno de ellos")]
    public List<TipoCombustible> combustibles;     //tipo del combustible
    [Tooltip("Habilidad de combustible activa")]
    public HabilidadCombustible habilidadCombustible; //variable que almacena la habilidad del cumbustible activo
    [Tooltip("Variable que controla si la nave está planeando o no")]
    public bool isPlanning = false;//variable de control. Si es true la nave está planeando
    [Tooltip("Pon la reducción de daño por colisión")]
    public float collisionDamageReduction = 0.8f;
    [Tooltip("Pon el prefab de la explosión")]
    public GameObject explosionPrefab;
    [Tooltip("Pon el tiempo que se queda la cámaras antes de mostrar el game over")]
    public float deathTime = 5;
    [Tooltip("Pon la cámara de la nave")]
    public Camera myCamera;
    public TrailRenderer trail;
    public Combustible combustible;
    public GameObject victoryImage;
    public GameObject hudCanvas;
    public AudioListener audioListener;
    public GameObject shipCentre;
    public int position;
    public Text positionText;
    public bool isIA = false;
    public bool isHologram = false;

    public int combustibleActivo = 0; //combustible activo, se usa como index para la lista "combustibles"
    private Stats stats;    //variable con las stats de la nave
    private NaveController controller;  //script con el controlador de la nave
    private Maneuverability maneuverability;
    private InputManager inputManager;
    public Rigidbody rb { get; set; }
    private PlanningManager planningManager;
    private NaveAnimationManager animationManager;
    private UIManager uiManager;
    private Dash dash;
    private SynchronizableColor trailColor;
    private EffectManager effectManager;
    
    private bool fuelInLeft = false;
    private bool fuelInRight = false;
    

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        stats = GetComponent<Stats>();
        controller = GetComponent<NaveController>();
        maneuverability = GetComponent<Maneuverability>();
        planningManager = GetComponent<PlanningManager>();
        dash = GetComponent<Dash>();
        animationManager = GetComponent<NaveAnimationManager>();
        uiManager = GetComponent<UIManager>();
        trailColor = new SynchronizableColor();
        effectManager = GetComponent<EffectManager>();
        AddPieceStats();
        AsignarCombustibleInicial();
        if(isIA)
        {

        }
        else
        {
            if (!photonView.isMine)
            {
                DisableComponents();
            }
            gameObject.name = gameObject.name + " " + photonView.owner.NickName;
        }
        
        trailColor.ToSynchronizable(trail.material.color);
    }

    private void OnEnable()
    {
        GameManager.navesList.Add(this);

    }

    private void OnDisable()
    {
        GameManager.navesList.Remove(this);
    }

    public void DisableComponents()
    {
        controller.enabled = false;
        planningManager.enabled = false;
        dash.enabled = false;
        animationManager.enabled = false;
        uiManager.enabled = false;
        hudCanvas.SetActive(false);
        audioListener.enabled = false;
    }
    

    public void AddPieceStats()
    {
        foreach(Pieza p in controller.piezas)
        {
            p.OnPieceDestroyed += stats.OnPieceDestroyed;
            p.OnPieceDestroyed += maneuverability.OnPieceDestroyed;
            foreach (Combustible c in GetComponents<Combustible>())
            {

            }
        }
    }

    private void Update()
    {
        FuelManager();
        if (!photonView.isMine) return;
        positionText.text = position.ToString() + "º";
        //combustible.PasiveConsumption();
    }

    private void AsignarCombustibleInicial()
    {
        //asignar el combustible que elija el jugador
        try
        {
            //habrá que cambiarlo para poner el combustible que elija el jugador
            habilidadCombustible = GetComponent(combustibles[0].ToString()) as HabilidadCombustible;
            combustibleActivo = 0;
        }
        catch
        {
            throw new Exception("Fallo al cargar habilidad de combustible");
        }
    }
    

    private void FuelManager()
    {
        if (PauseManager.inPause) return;

        combustible = habilidadCombustible.combustible;
        Vector3 locVel = GetComponent<NaveController>().modelTransform.InverseTransformDirection(rb.velocity);
        if (locVel.z <= 2)
        {
            trail.enabled = false;
        }
        else
        {
            trail.enabled = true;
        }

        if (trail.enabled)
        {
            trailColor.ToSynchronizable(habilidadCombustible.combustible.color);
            trail.material.color = trailColor.ToColor();
        }
        if (!photonView.isMine) return;
        /*Direction fuelSide = ChangeFuelManager();
        //cambiar entre los distintos combustibles
        if (fuelSide == Direction.Left)
        {
            try
            {
                combustibleActivo -= 1;
                if (combustibleActivo < 0) //comprueba que no se salga del límite del array
                {
                    combustibleActivo = combustibles.Count - 1;
                }
                habilidadCombustible = GetComponent(combustibles[combustibleActivo].ToString()) as HabilidadCombustible;
                photonView.RPC("ChangeHabilidadCombustible", PhotonTargets.OthersBuffered, (byte)habilidadCombustible.tipoCombustible);


            }
            catch
            {
                throw new Exception("Fallo al cambiar habilidad de combustible");
            }
        }
        if (fuelSide == Direction.Right || combustible.currentAmmount <= 0)
        {
            //print("entra en right");
            try
            {
                combustibleActivo += 1;
                if (combustibleActivo >= combustibles.Count) //comprueba que no se salga del límite del array
                {
                    combustibleActivo = 0;
                }
                habilidadCombustible = GetComponent(combustibles[combustibleActivo].ToString()) as HabilidadCombustible;

                photonView.RPC("ChangeHabilidadCombustible", PhotonTargets.OthersBuffered, (byte)habilidadCombustible.tipoCombustible);
            }
            catch
            {
                throw new Exception("Fallo al cambiar habilidad de combustible");
            }
        }

        if (inputManager.UseFuel())
        {
            habilidadCombustible.Use();
        }*/
    }

    [PunRPC]
    public void ChangeHabilidadCombustible(byte combustibleType)
    {
        habilidadCombustible = FindCombustibleByType((TipoCombustible)combustibleType);
    }

    private HabilidadCombustible FindCombustibleByType(TipoCombustible type)
    {
        foreach (HabilidadCombustible c in GetComponents<HabilidadCombustible>())
        {
            if (c.tipoCombustible == type)
            {
                return c;
            }
        }
        return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHologram) return;
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Nave")
        {
            DamageManager dm = collision.contacts[0].thisCollider.gameObject.GetComponentInParent<DamageManager>();
            float impactForce = Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity);
            impactForce = Mathf.Clamp(impactForce, 0, float.MaxValue);
            if (collision.contacts[0].thisCollider.gameObject.GetComponentInParent<DamageManager>())
            {
                if(collision.contacts[0].thisCollider.gameObject.GetComponentInParent<DamageManager>().GetComponent<PhotonView>())
                {
                    PhotonView pv = collision.contacts[0].thisCollider.gameObject.GetComponentInParent<DamageManager>().GetComponent<PhotonView>();
                    pv.RPC("TakeDamage", PhotonTargets.AllBuffered, impactForce * GetComponent<Stats>().currentCollisionDamage * (1 / collisionDamageReduction), false);
                }
            }
            if (collision.gameObject.GetComponent<DamageManager>())
            {
                if(collision.gameObject.GetComponent<DamageManager>().GetComponent<PhotonView>())
                {
                    PhotonView pv = collision.gameObject.GetComponent<DamageManager>().GetComponent<PhotonView>();
                    pv.RPC("TakeDamage", PhotonTargets.AllBuffered, impactForce * collision.contacts[0].thisCollider.gameObject.GetComponentInParent<Stats>().currentCollisionDamage * (1 / collisionDamageReduction), false);
                }
            }
            else if (collision.gameObject.GetComponentInParent<DamageManager>())
            {
                if(collision.gameObject.GetComponentInParent<DamageManager>().GetComponent<PhotonView>())
                {

                    PhotonView pv = collision.gameObject.GetComponentInParent<DamageManager>().GetComponent<PhotonView>();
                    pv.RPC("TakeDamage", PhotonTargets.AllBuffered, impactForce * collision.contacts[0].thisCollider.gameObject.GetComponentInParent<Stats>().currentCollisionDamage * (1 / collisionDamageReduction), false);
                }
            }
        }


    }

    [PunRPC]
    public void ApplyForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    
    public void TakeDamage(float damage, bool weapon, string target, string ownerNickname)
    {
        if (ownerNickname != photonView.owner.NickName) return;
        foreach (DamageManager dm in GetComponentsInChildren<DamageManager>())
        {
            print(dm.damagedObject);
            if (dm.damagedObject.ToString() == target)
            {
                PhotonView pv = dm.GetComponent<PhotonView>();
                pv.RPC("TakeDamage", PhotonTargets.AllBuffered,damage, weapon);
                return;
            }
        }
    }

    public void SetWeaponObjectives(LayerMask layers)
    {

    }

    private Direction ChangeFuelManager()
    {
        if (inputManager.ChangeFuel() > 0)
        {
            if (!fuelInRight)
            {
                fuelInLeft = false;
                fuelInRight = true;
                return Direction.Right;
            }
        }
        if (inputManager.ChangeFuel() < 0)
        {
            if (!fuelInLeft)
            {
                fuelInRight = false;
                fuelInLeft = true;
                return Direction.Left;
            }
        }
        if (inputManager.ChangeFuel() == 0)
        {
            fuelInRight = false;
            fuelInLeft = false;
            return Direction.None;
        }

        return Direction.None;
    }

    public void OnShipDestroyed()
    {
        StartCoroutine(OnShipDestroyedCoroutine());
    }

    public IEnumerator OnShipDestroyedCoroutine()
    {
        yield return new WaitForEndOfFrame();
        if (photonView.isMine)
        {
            myCamera.GetComponent<CameraController>().naveDestruida = true;
            myCamera.gameObject.GetComponent<CameraController>().OnShipDestroyed(deathTime);

        }

        
        GameObject explosion = Instantiate(explosionPrefab, GetComponent<NaveController>().transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponentInChildren<ParticleSystem>().main.duration);
        if (photonView.isMine)
        {
            photonView.RPC("DestroyShip", PhotonTargets.All);
        }
    }

    [PunRPC]
    public void DestroyShip()
    {
        GameManager.navesList.Remove(this);
        Destroy(transform.parent.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //stream.SendNext(habilidadCombustible.combustible);
        }
        else
        {
            //habilidadCombustible.combustible = (Combustible)stream.ReceiveNext();
        }
    }


    private float CalculateImpactForce(Vector3 collisionNormal, Vector3 collisionVelocity)
    {
        return Vector3.Dot(collisionNormal, collisionVelocity);
    }
    

    public float DistanceToNextCheckpoint
    {
        get { return (transform.position - CheckpointManager.current.CurrentCheckpoint.transform.position).magnitude; }
    }

    public float ShieldRepairRecharge
    {
        get { return position > (totalJugadores / 2) ? Mathf.CeilToInt(rechargeAmmount / 2) : Mathf.CeilToInt(position > (totalJugadores / 2) ? (((float)(totalJugadores - (position)) / (float)(totalJugadores)) * (float)rechargeAmmount) : (((float)(totalJugadores - (position) + 1) / (float)(totalJugadores)) * (float)rechargeAmmount)); }
    }

    public float BoostJumpRecharge
    {
        get { return position > (totalJugadores / 2) ? Mathf.FloorToInt(rechargeAmmount / 2) : Mathf.FloorToInt((position > (totalJugadores / 2) ? (((float)((position) / (float)totalJugadores) * (float)rechargeAmmount)) : ((float)((position - 1) / (float)totalJugadores) * (float)rechargeAmmount))); }
    }

    public int totalJugadores
    {
        get { return GameManager.navesList.Count; }
    }

    public int rechargeAmmount
    {
        get { return CheckpointManager.current.rechargeAmmount; }
    }

    #region Stats

    public float DamageReduction
    {
        get { return stats.damageReduction + effectManager.DamageReduction; }
    }

    public float Acceleration
    {
        get { return maneuverability.AccelerationWithWeight + effectManager.Acceleration; }
    }

    public float Velocity
    {
        get { return maneuverability.MaxVelocity + effectManager.Velocity; }
    }

    public float Maneuver
    {
        get { return maneuverability.currentManeuver + effectManager.Maneuver; }
    }

    public float ShotDamage
    {
        get { return stats.shotDamage + effectManager.ShotDamage; }
    }



    #endregion

}
