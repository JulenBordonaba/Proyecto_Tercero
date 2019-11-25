using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerAbility : PlayerAbility
{
    [Tooltip("Pon el prefab de la chatarra")]
    public GameObject chatarraPrefab;
    [Tooltip("Pon el máximo de chatarras que el jugador puede tener activas a la vez")]
    public int maxChatarras = 3;
    [Tooltip("Pon el tronsform en el que spawneará la chatarra")]
    public Transform spawn;
    [Tooltip("Pon la fuerza con la que se impulsa la chatarra al ser lanzada")]
    public float throwForce = 10;

    private Transform modelTransform;
    public List<GameObject> chatarras = new List<GameObject>();

    private void Start()
    {
        modelTransform = GetComponent<NaveController>().modelTransform;
    }

    public override void Use()
    {
        base.Use();
        if (inCooldown) return;
        inCooldown = true;
        StartCoroutine(Cooldown());
        GameObject nuevo = Instantiate(chatarraPrefab, spawn.position, Quaternion.identity);
        nuevo.GetComponent<Rigidbody>().AddForce(Vector3.up * throwForce, ForceMode.Impulse);
        nuevo.GetComponent<Rigidbody>().AddForce(-modelTransform.forward * throwForce, ForceMode.Impulse);
        ActualizarLista(nuevo);

    }

    private void ActualizarLista(GameObject nuevo)
    {
        if(chatarras.Count<maxChatarras)
        {
            chatarras.Add(nuevo);
        }
        else if(chatarras.Count==maxChatarras)
        {
            List<GameObject> aux = new List<GameObject>();
            for (int i = 1; i<maxChatarras; i++)
            {
                aux.Add(chatarras[i]);
            }
            aux.Add(nuevo);
            Destroy(chatarras[0]);
            chatarras = aux;
        }
    }
}
