using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerAbility : PlayerAbility
{
    [Tooltip("Pon el máximo de chatarras que el jugador puede tener activas a la vez")]
    public int maxChatarras = 3;
    [Tooltip("Pon el tronsform en el que spawneará la chatarra")]
    public Transform spawn;

    private Transform modelTransform;
    public List<GameObject> chatarras = new List<GameObject>();

    private void Start()
    {
        modelTransform = GetComponent<NaveController>().modelTransform;
        effectManager = GetComponent<EffectManager>();
    }

    public override void Use(float forcedCooldown)
    {
        //base.Use(forcedCooldown);
        //if (effectManager.SilenceAbilities) return;
        //if (inCooldown) return;
        //inCooldown = true;
        //StartCoroutine(Cooldown(cooldown * forcedCooldown));
        //GameObject nuevo = Instantiate(chatarraPrefab, spawn.position, Quaternion.identity);
        //nuevo.GetComponent<Rigidbody>().AddForce(Vector3.up * throwForce, ForceMode.Impulse);
        //nuevo.GetComponent<Rigidbody>().AddForce(-modelTransform.forward * throwForce, ForceMode.Impulse);
        //ActualizarLista(nuevo);
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
