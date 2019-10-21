using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationManager : MonoBehaviour
{
    [Tooltip("Pon la altura a la que se mantendrá la nave a flote")]
    public float levitationHeight = 20;
    [Tooltip("Pon la fuerza con la que se impulsa la nave para mantenerla a flote")]
    public float levitationForce = 10;
    [Tooltip("Pon la altura a la que la nave empieza a corregir su rotación para alinearse con el suelo")]
    public float startCorrectionHeight = 50;
    [Tooltip("damping")]
    public float damping = 2;

    public float upDamping = 2;

    private Rigidbody rb;   //rigidbody de la nave

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Levitate();
    }

    private void Levitate()
    {
        Ray ray = new Ray();
        RaycastHit hit;

        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);


        ray.origin = GetComponent<NaveController>().modelTransform.position + Vector3.ClampMagnitude((locVel.z * GetComponent<NaveController>().modelTransform.forward), 5f);
        ray.direction = -Vector3.up;

        Debug.DrawRay(ray.origin, -Vector3.up, Color.green);  //dibujamos el resultado del raycast

        //lanzamos un raycast hacia el suelo
        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Floor")))
        {


            Vector3 rayDistance = ray.origin - hit.point; //guardamos la distancia del raycast

            float diference = rayDistance.magnitude - levitationHeight; //diferencia de altura entre la nave y la altura en la que queremos que esté



            
            //se le añade una fuerza para que flote a la altura que queremos
            /*if (rayDistance.magnitude < levitationHeight / 2)
            {
                rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * 20), ForceMode.Acceleration);
            }
            else */if (rayDistance.magnitude < levitationHeight)
            {
                rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * 1), ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * 1), ForceMode.Acceleration);
            }


            Rotaion(hit, rayDistance.magnitude);


            //si esta por debajo de la altura que queremos y rb.velocity.y<0  si esta por encima de la altura que queremos y rb.velocity.y>0 reducimos la velocidad del eje y
            if ((diference > 0 && rb.velocity.y > 0) || (diference < 0 && rb.velocity.y < 0))
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.9f, rb.velocity.z);
            }

        }

    }

    //Modificamos la rotación de la nave
    private void Rotaion(RaycastHit hit, float rayDistance)
    {
        if(rayDistance>startCorrectionHeight)
        {
            Quaternion interpolation;
            interpolation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 0),Time.deltaTime * upDamping);
            transform.localRotation = interpolation;
        }
        else
        {
            Quaternion quaternionRot = transform.localRotation; //guardamos la rotación actual en quaternions

            transform.up = hit.normal; //hacemos que la nave este perpendicular a la normal del punto donde ha colisionado el raycast

            Quaternion quatNewRot = transform.localRotation;  //guardamos la rotación en quaternions despues de corregirla 
            Quaternion interpolation;
            //hacemos una interpolación entre la rotación inicial y la final en relación a la distancia al suelo

            //interpolation = Quaternion.Lerp(quaternionRot, quatNewRot, (1 - ((rayDistance - levitationHeight) / startCorrectionHeight)) * (1 / rayDistance));
            interpolation = Quaternion.Lerp(quaternionRot, quatNewRot, (rayDistance - levitationHeight) < levitationHeight*0.2f ? Time.deltaTime*damping : (1 - ((rayDistance - levitationHeight) / startCorrectionHeight)) * (1 / rayDistance));

            //igualamos la rotación a el resultado de la interpolación
            transform.localRotation = interpolation;
        }
        
    }
}
