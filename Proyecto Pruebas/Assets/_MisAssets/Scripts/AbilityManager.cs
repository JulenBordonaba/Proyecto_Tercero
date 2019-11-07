using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [Tooltip("Pon los módulos de la nave")]
    public List<AbilityModule> modules = new List<AbilityModule>();
    [Tooltip("Pon la habilidad pasiva de la nave")]
    public PasiveAbility pasive;
    [Tooltip("Pon la habilidad del jugador")]
    public HabilidadPersonaje playerAbilityType;

    private PlayerAbility playerAbility;
    private ShipAbility shipAbility;    //habilidad de la nave


    private void Start()
    {
        shipAbility = GetComponent<ShipAbility>();
        SetPlayerAbility();
    }


    // Update is called once per frame
    void Update()
    {
        if(InputManager.ShipAbility())
        {
            shipAbility.Use();            
        }
        if(InputManager.PlayerAbility())
        {
            playerAbility.Use();
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
}
