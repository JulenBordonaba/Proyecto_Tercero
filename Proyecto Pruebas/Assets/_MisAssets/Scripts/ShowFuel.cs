using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFuel : MonoBehaviour
{
    public Text fuelText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fuelText.text = GetComponent<NaveManager>().combustibles[GetComponent<NaveManager>().combustibleActivo].ToString();
    }
}
