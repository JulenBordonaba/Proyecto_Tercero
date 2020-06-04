using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankAbility : PlayerAbility
{
    public Camera myCamera;

    public float targetRadius = 100f;
    public Image targetImage;
    public Transform proyectileSpawn;
    public float proyectileVelocity = 500;

    private NaveManager currentTarget;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        effectManager = GetComponent<EffectManager>();
    }

    public override void Use(float forcedCooldown)
    {
        base.Use(forcedCooldown);
        if (effectManager.SilenceAbilities) return;

        if (!inCooldown)
        {
            //Cooldown
            StartCoroutine(Cooldown(cooldown * forcedCooldown));
            inCooldown = true;

            //efectos
            audiSource.Play();

            //Crear projectil
            GameObject proyectile = PhotonNetwork.Instantiate("TankProyectile", proyectileSpawn.position, myCamera.transform.rotation,0);
            proyectile.GetComponent<PhotonView>().RPC("SetTarget", PhotonTargets.All,(currentTarget? currentTarget.photonView.owner.NickName : ""));
            proyectile.GetComponent<PhotonView>().RPC("SetVelocity", PhotonTargets.All, rb.velocity.magnitude + proyectileVelocity);
            proyectile.GetComponent<PhotonView>().RPC("SetDirection", PhotonTargets.All, proyectile.transform.forward);

        }

    }

    private void Update()
    {
        CalculateTarget();
    }

    void CalculateTarget()
    {
        if (inCooldown) return;
        
        List<NaveManager> naveTargets = new List<NaveManager>();
        foreach(NaveManager nm in GameManager.navesList)
        {
            Vector3 naveScreenPosition = myCamera.WorldToScreenPoint(nm.transform.position);
            if(CircleCollision(ScreenMiddle.x, ScreenMiddle.y,targetRadius,naveScreenPosition.x,naveScreenPosition.y))
            {
                naveTargets.Add(nm);
            }
        }

        NaveManager target = MostCenteredShip(naveTargets, ScreenMiddle);
        currentTarget = target;

        ShowTarget(target);
        

    }

    void ShowTarget(NaveManager target)
    {
        if (target != null)
        {
            if (!targetImage.gameObject.activeInHierarchy)
            {
                targetImage.gameObject.SetActive(true);
            }
            targetImage.rectTransform.localPosition = myCamera.WorldToScreenPoint(target.transform.position)- new Vector3(ScreenMiddle.x, ScreenMiddle.y,0);
        }
        else
        {
            if (targetImage.gameObject.activeInHierarchy)
            {
                targetImage.gameObject.SetActive(false);
            }
        }
    }

    NaveManager MostCenteredShip(List<NaveManager> naveTargets, Vector2 screenMiddle)
    {
        if (naveTargets.Count <= 0) return null;
        NaveManager _mostCentered = null;
        float currentNearestDistance = Mathf.Infinity;

        foreach(NaveManager nm in naveTargets)
        {
            Vector3 naveScreenPosition = myCamera.WorldToScreenPoint(nm.transform.position);
            float centreDistance = Vector2.Distance(screenMiddle, new Vector2(naveScreenPosition.x, naveScreenPosition.y));
            if ( centreDistance<currentNearestDistance)
            {
                currentNearestDistance = centreDistance;
                _mostCentered = nm;
            }
        }

        return _mostCentered;
    }

    private bool CircleCollision(float x, float y, float r, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow((x2 - x), 2) - Mathf.Pow((y2 - y), 2)) < r;
    }

    Vector2 ScreenMiddle
    {
        get { return new Vector2(Screen.width * 0.5f, Screen.height * 0.5f); }
    }

}
