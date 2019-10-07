using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaCombustible : MonoBehaviour
{
    public string[] combustibles;
    public HabilidadCombustible habilidadCombustible;
    private int combustibleActivo = 0;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            habilidadCombustible = GetComponent(combustibles[0]) as HabilidadCombustible;
            combustibleActivo = 0;
        }
        catch
        {
            throw new Exception("Fallo al cargar habilidad de combustible");
        }
    }

    // Update is called once per frame
    void Update()
    {

        //código cutre para pruebas

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            try
            {
                combustibleActivo -= 1;
                if(combustibleActivo<0) //comprueba que no se salga del límite del array
                {
                    combustibleActivo = combustibles.Length - 1;
                }
                habilidadCombustible = GetComponent(combustibles[combustibleActivo]) as HabilidadCombustible;


            }
            catch
            {
                throw new Exception("Fallo al cambiar habilidad de combustible");
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            try
            {
                combustibleActivo += 1;
                if (combustibleActivo >= combustibles.Length) //comprueba que no se salga del límite del array
                {
                    combustibleActivo =  0;
                }
                habilidadCombustible = GetComponent(combustibles[combustibleActivo]) as HabilidadCombustible;


            }
            catch
            {
                throw new Exception("Fallo al cambiar habilidad de combustible");
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            habilidadCombustible.Use();
        }

        
    }
}
