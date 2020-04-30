﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : Photon.PunBehaviour
{
    [Tooltip("Pon los módulos de la nave")]
    public List<AbilityModule> modules = new List<AbilityModule>();
    [Tooltip("Pon la habilidad pasiva de la nave")]
    public PasiveAbility pasive;
    [Tooltip("Pon la habilidad del jugador")]
    public HabilidadPersonaje playerAbilityType;
    [HideInInspector]
    public PlayerAbility playerAbility;
    [HideInInspector]
    public ShipAbility shipAbility;    //habilidad de la nave
    private InputManager inputManager;


    private void Start()
    {
        shipAbility = GetComponent<ShipAbility>();
        SetPlayerAbility();
        inputManager = GetComponent<InputManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine) return;
        if (PauseManager.inPause) return;
        if(inputManager.ShipAbility())
        {
            shipAbility.Use(1);            
        }
        if(inputManager.PlayerAbility())
        {
            playerAbility.Use(1);
        }
    }

    private void SetPlayerAbility()
    {
        foreach(PlayerAbility pa in GetComponents<PlayerAbility>())
        {
            if(pa.clase==playerAbilityType)
            {
                playerAbility = pa;
            }
        }
    }

    public void ForceUse()
    {
        photonView.RPC("ForceUseRPC", PhotonTargets.All, photonView.owner.NickName);
    }

    [PunRPC]
    public void ForceUseRPC(string _nickname, float forcedCooldown)
    {

        if (PhotonNetwork.player.NickName != _nickname) return;
        shipAbility.Use(forcedCooldown);
        playerAbility.Use(forcedCooldown);
    }
}
