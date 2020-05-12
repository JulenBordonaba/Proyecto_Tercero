using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FuelIcon : MonoBehaviour
{
    public NaveManager naveManager;

    public TipoCombustible fuelType;

    [HideInInspector]
    public HabilidadCombustible fuel;
    [HideInInspector]
    public Image maskImage;
    // Start is called before the first frame update
    void Start()
    {
        maskImage = GetComponent<Image>();

        foreach(HabilidadCombustible c in naveManager.gameObject.GetComponents<HabilidadCombustible>())
        {
            if(c.tipoCombustible==fuelType)
            {
                fuel = c;
                break;
            }
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
        if (fuel.inCooldown)
        {
            maskImage.fillAmount = fuel.currentCooldown / fuel.maxCooldown;
        }
        else
        {
            maskImage.fillAmount = 0;
        }
    }




}
