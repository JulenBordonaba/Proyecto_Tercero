using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    [Tooltip("Pon la cámara de la nave")]
    public Camera myCamera;
    [Tooltip("Pon la distancia del raycast de la cámara a la que apunta el disparo si se esta apuntando al aire")]
    public float shotDistance = 200;
    [Tooltip("Pon el tiempo que tarda en disparar desde el último disparo")]
    public float cooldown = 0.1f;
    [Tooltip("Pon en false para bloquear la rotación en el eje x")]
    public bool x = true;
    [Tooltip("Pon en false para bloquear la rotación en el eje y")]
    public bool y = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        if(InputManager.Shot())
        {
            Shot();
        }
    }

    public void Shot()
    {

    }

    public void Aim()
    {
        Ray ray = new Ray();
        RaycastHit hit;
        ray.origin = myCamera.transform.position;
        ray.direction = myCamera.transform.forward;
        if(Physics.Raycast(ray, out hit,shotDistance))
        {
            //weapon.transform.LookAt(hit.point);
        }
        else
        {
            //weapon.transform.LookAt(ray.origin+ray.direction*shotDistance);
        }
    }
}
