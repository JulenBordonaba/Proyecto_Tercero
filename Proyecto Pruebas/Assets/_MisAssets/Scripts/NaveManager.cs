using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveManager : MonoBehaviour
{

    [Tooltip("Pon el numero de combustibles correspondiente en Size. Luego elige una de las 4 opciones para cada uno de ellos")]
    public List<TipoCombustible> combustibles;     //tipo del combustible
    public HabilidadCombustible habilidadCombustible;
    private int combustibleActivo = 0;
    public bool inShield = false; //variable de control. Si es true el escudo está activo y no recibe daño

    private void AsignarCombustibleInicial()
    {

    }
    private void CombustibleManager()
    {

    }
}
