using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerAbility : PlayerAbility
{
    [Tooltip("Pon el máximo de chatarras que el jugador puede tener activas a la vez")]
    public int maxChatarras = 3;
    [Tooltip("Pon el tronsform en el que spawneará la chatarra")]
    public Transform spawn;
    public float maxRayDistance = 50f;
    public LayerMask hitLayers;

    private Transform modelTransform;
    public List<GameObject> chatarras = new List<GameObject>();

    private void Start()
    {
        modelTransform = GetComponent<NaveController>().modelTransform;
        effectManager = GetComponent<EffectManager>();
    }

    public override void Use(float forcedCooldown)
    {
        base.Use(forcedCooldown);
        if (effectManager.SilenceAbilities) return;
        if (inCooldown) return;
        inCooldown = true;
        StartCoroutine(Cooldown(cooldown * forcedCooldown));

        Ray ray = new Ray();
        ray.origin = spawn.position;
        ray.direction = -Vector3.up;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRayDistance, hitLayers))
        {
            GameObject nuevo = PhotonNetwork.Instantiate("Chatarra_Habilidad_Chatarrero", spawn.position, Quaternion.identity, 0);
            ActualizarLista(nuevo);
        }

        
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
