using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : Photon.PunBehaviour
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

    public bool clampRotation = true;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine) return;
        Aim();
    }


    public void Aim()
    {
        Ray ray = new Ray();
        RaycastHit[] hits;
        ray.origin = myCamera.transform.position;
        ray.direction = myCamera.transform.forward;
        Quaternion rotation = transform.localRotation;
        hits = Physics.RaycastAll(ray, shotDistance, layers);
        if (hits.Length>0)
        {
            RaycastHit nearestHit = hits[0];
            foreach(RaycastHit hit in hits)
            {
                if(!hit.collider.gameObject.GetComponentInParent<PhotonView>() || (hit.collider.gameObject.GetComponentInParent<PhotonView>() && !hit.collider.gameObject.GetComponentInParent<PhotonView>().isMine))
                {
                    if((nearestHit.point - ray.origin).magnitude> (hit.point - ray.origin).magnitude)
                    {
                        nearestHit = hit;
                    }
                }

                transform.LookAt(nearestHit.point);
                if(clampRotation)
                {
                    float xrot = transform.localEulerAngles.x;
                    float yrot = transform.localEulerAngles.y;
                    xrot = Global.ClampAngle(xrot, minX, maxX);
                    yrot = Global.ClampAngle(yrot, minY, maxY);
                    transform.localEulerAngles = new Vector3(xrot, yrot, transform.localEulerAngles.z);
                }
                Debug.DrawRay(ray.origin, ray.direction * (nearestHit.point - ray.origin).magnitude, Color.red);
            }
        }
        else
        {
            transform.LookAt(ray.origin + ray.direction * shotDistance);
            if (clampRotation)
            {
                float xrot = transform.localEulerAngles.x;
                float yrot = transform.localEulerAngles.y;
                xrot = Global.ClampAngle(xrot, minX, maxX);
                yrot = Global.ClampAngle(yrot, minY, maxY);
                transform.localEulerAngles = new Vector3(xrot, yrot, transform.localEulerAngles.z);
            }
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red);
        }

        Quaternion finalRotation = Quaternion.Euler(x ? transform.localEulerAngles.x : Mathf.Clamp( rotation.eulerAngles.x,minX,maxX), y ? transform.localEulerAngles.y : rotation.eulerAngles.y, 0);
        transform.localRotation = finalRotation;

    }
}
