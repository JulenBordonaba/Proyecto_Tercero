using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AbilityIcon : MonoBehaviour
{
    public AbilityManager abilityManager;

    public AbilityType abilityType = AbilityType.Player;

    [HideInInspector]
    public Ability ability;
    [HideInInspector]
    public Image maskImage;
    // Start is called before the first frame update
    void Start()
    {
        maskImage = GetComponent<Image>();
        

        if(abilityType== AbilityType.Player)
        {
            ability = abilityManager.playerAbility;
        }
        else if(abilityType== AbilityType.Ship)
        {
            ability = abilityManager.shipAbility;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!maskImage) return;


        UpdateMask();

    }


    void UpdateMask()
    {
        if(ability.inCooldown)
        {
            maskImage.fillAmount = ability.currentCooldown / ability.cooldown;
        }
        else
        {
            maskImage.fillAmount = 0;
        }
    }




}
