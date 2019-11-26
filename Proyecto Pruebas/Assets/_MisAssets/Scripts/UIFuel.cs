using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFuel : MonoBehaviour
{
    [Tooltip("Pon el objeto que contiene los combustibles")]
    public GameObject objetoCombustibles;
    [Tooltip("Pon el combustible que le corresponde")]
    public TipoCombustible tipoCombustible;
    [Tooltip("Pon el objeto del color")]
    public Image colorObject;
    [Tooltip("Pon la altura mínima a la que puede bajar el combustible sin que se salga de la barra")]
    public float minFuelY = -80;


    private Combustible combustible;
    private NaveManager naveManager;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        GetFuel();
        naveManager = objetoCombustibles.GetComponent<NaveManager>();
        colorObject.color = combustible.color;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectFuelBar();
        FuelBarHeight();
    }

    private void FuelBarHeight()
    {
        RectTransform rect = colorObject.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(rect.localPosition.x, Mathf.Lerp(0, minFuelY, (1-FuelAmmount)), rect.localPosition.z);
    }

    public void SelectFuelBar()
    {
        anim.SetBool("selected", IsSelected);
    }

    private void GetFuel()
    {
        foreach (Combustible c in objetoCombustibles.GetComponentsInChildren<Combustible>())
        {
            if(c.tipoCombustible==tipoCombustible)
            {
                combustible = c;
                return;
            }
        }
        
    }

    public bool IsSelected
    {
        get { return combustible == naveManager.combustible; }
    }

    public float FuelAmmount
    {
        get { return combustible.currentAmmount / combustible.deposit; }
    }
}
