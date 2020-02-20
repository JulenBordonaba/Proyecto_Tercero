using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PruebaFormula : MonoBehaviour
{
    public Text boostJumpText;
    public Text shieldRepairText;
    public Text sumaText;

    public int position = 1;
    public int totalJugadores = 2;
    public int rechargeAmmount = 4;

    public float BoostJumpRecharge
    {
        get { return Mathf.CeilToInt(position > (totalJugadores / 2) ? (((float)(totalJugadores - (position)) / (float)(totalJugadores)) * (float)rechargeAmmount) : (((float)(totalJugadores - (position) ) / (float)(totalJugadores)) * (float)rechargeAmmount)); }
    }

    public float ShieldRepairRecharge
    {
        get { return Mathf.FloorToInt((position > (totalJugadores / 2) ? (((float)((position) / (float)totalJugadores) * (float)rechargeAmmount)) : ((float)((position - 1) / (float)totalJugadores) * (float)rechargeAmmount))); }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        boostJumpText.text = "BoostJump: " + BoostJumpRecharge.ToString();
        shieldRepairText.text = "ShieldRepair: " + ShieldRepairRecharge.ToString();
        sumaText.text = "Suma: " + (BoostJumpRecharge + ShieldRepairRecharge).ToString();
    }
}
