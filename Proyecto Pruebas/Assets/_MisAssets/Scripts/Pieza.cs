using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pieza : Photon.PunBehaviour {

    
    [Tooltip("Importancia que tiene la pieza, el porcentage en el que aumenta los stats de la nave, Rango:0-100, si es el núcleo poner a 0"), Range(0, 100)]
    public float importancia = 25;
    [Tooltip("Pon el porcentaje de vida a partir del cual la pieza cambia a estado dañado")]
    [Range(0, 100)]
    public float damagedLimit = 50;
    [Tooltip("Pon el objeto de la pieza del HUD")]
    public GameObject piezaHUD;

    public event Action<float> OnPieceDestroyed;



    //[HideInInspector]
    public float currentHealth;
    [Tooltip("Esta variable hay que dejarla activada únicamente en la pieza nucleo de la nave")]
    public bool nucleo;
    [Tooltip("Variable que controla cuando esta rota la pieza")]
    public bool isBroken = false;
    public ShootWeapon[] armas;

    public float maxHealth;
    private GameObject piezaOk;
    private GameObject piezaBroken;
    private GameObject piezaDead;
    public enum PieceState { }

    // Use this for initialization
    void Start () {
        maxHealth = GetComponentInParent<Stats>().life;
        currentHealth = maxHealth;

        OnPieceDestroyed += PieceDestroyed;

        GetComponentInParent<Stats>().AddPieceValues(importancia);
        GetComponentInParent<Maneuverability>().AddPieceValues(importancia);

        piezaOk = transform.Find(gameObject.name + "_Ok").gameObject;
        piezaBroken = transform.Find(gameObject.name + "_Broken").gameObject;
        piezaDead = transform.Find(gameObject.name + "_Dead").gameObject;

    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Damage(0);
        }
    }


    private void PieceDestroyed(float _importancia)
    {
        isBroken = true;
        foreach(ShootWeapon sw in armas)
        {
            sw.enabled = false;
        }
        if(nucleo)
        {
            GetComponentInParent<NaveManager>().OnShipDestroyed();
        }
    }

    public void Damage(float ammount)
    {
        //print("damage pieza");
        if (currentHealth <= 0) return;
        //print("continua damage pieza");
        GetComponentInParent<Stats>().currentLife -= ammount;
        currentHealth -= ammount;
        piezaHUD.transform.SetAsLastSibling();
        piezaHUD.GetComponent<Animator>().SetTrigger("damage");
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        CheckState();
        if(currentHealth<=0)
        {
            OnPieceDestroyed.Invoke(importancia);
        }
    }

    public void CheckState()
    {
        if(currentHealth<=0)
        {
            piezaBroken.SetActive(false);
            piezaOk.SetActive(false);
            piezaDead.SetActive(true);
        }
        else if((currentHealth/maxHealth) < (damagedLimit/100))
        {
            piezaBroken.SetActive(true);
            piezaOk.SetActive(false);
            piezaDead.SetActive(false);
        }
        else
        {
            piezaBroken.SetActive(false);
            piezaOk.SetActive(true);
            piezaDead.SetActive(false);
        }
    }
    
    public float Importancia
    {
        get { return importancia / 100; }
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
        }
    }

}
