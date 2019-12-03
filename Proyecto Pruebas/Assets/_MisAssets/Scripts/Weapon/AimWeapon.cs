using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    [Tooltip("Pon la cámara de la nave")]
    public Camera myCamera;
    [Tooltip("Pon la distancia del raycast de la cámara a la que apunta el disparo si se esta apuntando al aire")]
    public float shotDistance = 200;
    [Tooltip("Pon en false para bloquear la rotación en el eje x")]
    public bool x = true;
    [Tooltip("Pon en false para bloquear la rotación en el eje y")]
    public bool y = true;
    [Tooltip("Pon las layers contra las que choca el raycast del disparo")]
    public LayerMask layers;
    [Header("Weapon Limits")]
    [Tooltip("Pon el valor mínimo de la rotación en x")]
    public float minX=-180;
    [Tooltip("Pon el valor máximo de la rotación en x")]
    public float maxX=180;
    [Tooltip("Pon el valor mínimo de la rotación en y")]
    public float minY=-180;
    [Tooltip("Pon el valor máximo de la rotación en y")]
    public float maxY=180;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Aim();
    }


    public void Aim()
    {
        Ray ray = new Ray();
        RaycastHit hit;
        ray.origin = myCamera.transform.position;
        ray.direction = myCamera.transform.forward;
        Quaternion rotation = transform.localRotation;
        if (Physics.Raycast(ray, out hit, shotDistance,layers))
        {
            transform.LookAt(hit.point);
            Debug.DrawRay(ray.origin, ray.direction * (hit.point - ray.origin).magnitude, Color.red);
        }
        else
        {
            transform.LookAt(ray.origin + ray.direction * shotDistance);
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red);
        }

        Quaternion finalRotation = Quaternion.Euler(x ? transform.localEulerAngles.x : Mathf.Clamp( rotation.eulerAngles.x,minX,maxX), y ? transform.localEulerAngles.y : rotation.eulerAngles.y, 0);
        transform.localRotation = finalRotation;

    }
}
