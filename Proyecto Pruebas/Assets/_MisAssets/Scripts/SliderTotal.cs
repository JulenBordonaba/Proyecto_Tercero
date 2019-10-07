using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Función para sumar los atributos y mostrarlos en los sliders

public class SliderTotal : MonoBehaviour
{
    
    private float health;
    private float weight;
    private float maxVel;
    private float acceleration;
    private float manoeuvrability;
    private float damage;
    private float recoil;
    private float turbo;
    private float skid;
    private float sideDash;

    public Slider[] attributes;

    public Text[] attributesText;

    private void Start()
    {
        UpdateSliderValue();
        ActualSliderValue();
        
    }
    private void Update()
    {
        ActualSliderValue();

    }


    //Actualiza el valor de los atributos totales
    public void UpdateSliderValue()
    {
        ResetValues();
        for (int i = 0; i < transform.childCount; i++)
        {
           /* health += transform.GetChild(i).GetComponent<Pieza>().vida;
            weight += transform.GetChild(i).GetComponent<Pieza>().peso;
            maxVel += transform.GetChild(i).GetComponent<Pieza>().velocidad;
            acceleration += transform.GetChild(i).GetComponent<Pieza>().aceleracion;
            manoeuvrability += transform.GetChild(i).GetComponent<Pieza>().manejo;
            damage += transform.GetChild(i).GetComponent<Pieza>().daño;
            recoil += transform.GetChild(i).GetComponent<Pieza>().rebufo;
            turbo += transform.GetChild(i).GetComponent<Pieza>().turbo;
            skid += transform.GetChild(i).GetComponent<Pieza>().derrape;
            sideDash += transform.GetChild(i).GetComponent<Pieza>().dashLateral;*/
        }
    }
    
    public void ResetValues()
    {
        health = 0;
        weight = 0;
        maxVel = 0;
        acceleration = 0;
        manoeuvrability = 0;
        damage = 0;
        recoil = 0;
        turbo = 0;
        skid = 0;
        sideDash = 0;
    }

    
    //Muestra el valor actual y actualiza los slider
    public void ActualSliderValue()
    {

        attributes[0].value = health / (300 * transform.childCount);
        attributesText[0].text = (attributes[0].value*100).ToString("F2");

        attributes[1].value = weight / (100 * transform.childCount);
        attributesText[1].text = (attributes[1].value * 100).ToString("F2");

        attributes[2].value = maxVel / (100 * transform.childCount);
        attributesText[2].text = (attributes[2].value * 100).ToString("F2");

        attributes[3].value = acceleration / (100 * transform.childCount);
        attributesText[3].text = (attributes[3].value * 100).ToString("F2");

        attributes[4].value = manoeuvrability / (100 * transform.childCount);
        attributesText[4].text = (attributes[4].value * 100).ToString("F2");

        attributes[5].value = damage / (100 * transform.childCount);
        attributesText[5].text = (attributes[5].value * 100).ToString("F2");

        attributes[6].value = recoil / (100 * transform.childCount);
        attributesText[6].text = (attributes[6].value * 100).ToString("F2");

        attributes[7].value = turbo / (100 * transform.childCount);
        attributesText[7].text = (attributes[7].value * 100).ToString("F2");

        attributes[8].value = skid / (100 * transform.childCount);
        attributesText[8].text = (attributes[8].value * 100).ToString("F2");

        attributes[9].value = sideDash / (100 * transform.childCount);
        attributesText[9].text = (attributes[9].value * 100).ToString("F2");
    }

}
