using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationManager : Photon.PunBehaviour
{
    [Tooltip("Pon la altura a la que se mantendrá la nave a flote")]
    public float levitationHeight = 20;
    [Tooltip("Pon la fuerza con la que se impulsa la nave para mantenerla a flote")]
    public float levitationForce = 10;
    [Tooltip("Pon la altura a la que la nave empieza a corregir su rotación para alinearse con el suelo")]
    public float startCorrectionHeight = 50;
    [Tooltip("Pon la distancia máxima a la que el raycast disparara respecto al centro de la nave")]
    public float rayOffset = 8;
    [Tooltip("pon la variable que aumenta o disminuye el tiempo que tarda la nave en corregir su rotación cuando está cerca del suelo")]
    public float damping = 2;
    [Tooltip("pon la variable que aumenta o disminuye el tiempo que tarda la nave en corregir su rotación cuando está lejos del suelo")]
    public float upDamping = 2;
    [Tooltip("Pon el impulso vertical extra que se le aplica a la nave para que no parezca que cae a cámara lenta")]
    public float extraFallImpulse;  //impulso extra que se le aplica a la nave al caer para que no parezca que cae a cámara lenta
    [Tooltip("Pon las layers con las que puede colisionar el linecast para definir el origen del raycast y que no se quede encallado en otros objetos, pon las layers de los objetos")]
    [Range(0f, 1f)]
    public float maxSlope = 0.66f;

    private Rigidbody rb;   //rigidbody de la nave
    private NaveController controller;
    private NaveManager naveManager;
    private NaveAnimationManager animationManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<NaveController>();
        naveManager = GetComponent<NaveManager>();
        animationManager = GetComponent<NaveAnimationManager>();
    }

    private void FixedUpdate()
    {
        if (!photonView.isMine) return;
        Levitate();
    }

    private void Levitate()
    {
        if (PauseManager.inPause) return;
        Ray ray = new Ray();
        RaycastHit hit;

        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);




        ray.origin = controller.modelTransform.position + Vector3.ClampMagnitude((locVel.z * controller.modelTransform.forward * rayOffset / 5), rayOffset);
        //hacer una linecast desde el centro hasta donde debería estar el origen del raycast, si el linecast colisiona con algo que no sea la nave colocar el origen del raycast en dicha posición
        //Physics.Linecast(GetComponent<NaveController>().modelTransform.position,ray.origin,)

        ray.direction = -Vector3.up;


        Ray frontRay = new Ray();
        frontRay.origin = controller.modelTransform.position;
        frontRay.direction = controller.modelTransform.forward;
        RaycastHit frontHit;
        if (Physics.Raycast(frontRay, out frontHit, rayOffset * 1.5f, LayerMask.GetMask("Floor")))
        {
            ray.origin = controller.modelTransform.position;
        }

        Debug.DrawRay(ray.origin, -Vector3.up * 100, Color.green);  //dibujamos el resultado del raycast

        //print(GetComponent<NaveController>().modelTransform.up);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor"), QueryTriggerInteraction.UseGlobal))
        {
            //print(hit.transform.gameObject.name);

            Debug.DrawRay(hit.point, hit.normal * 1000f, Color.cyan);
            Vector3 rayPath = ray.origin - hit.point;
            //float rayDistance = Mathf.Clamp(rayPath.magnitude - rayDifference, 1, Mathf.Infinity); //guardamos la distancia del raycast
            float rayDistance = Mathf.Clamp(rayPath.magnitude, 1, Mathf.Infinity);
            //print(rayDistance);

            float diference = rayDistance - levitationHeight; //diferencia de altura entre la nave y la altura en la que queremos que esté

            /*Vector3 rayPath = ray.origin - hit.point; //guardamos la distancia del raycast

            float rayDistance = rayPath.magnitude;

            float diference = rayDistance - levitationHeight; //diferencia de altura entre la nave y la altura en la que queremos que esté
            print(rayDistance);*/

            //se le añade una fuerza para que flote a la altura que queremos
            if (rayDistance < levitationHeight)
            {
                //rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance) * (levitationHeight - rayDistance)) * Time.deltaTime * 30 * 1, ForceMode.Acceleration);
            }
            else
            {
                //rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance) * (levitationHeight - rayDistance)) * Time.deltaTime * 30 * -1, ForceMode.Acceleration);
            }

            if (hit.normal.y > maxSlope)
            {
                rb.AddForce(-Vector3.up * (1 - hit.normal.y) * levitationForce * Time.fixedDeltaTime * 30, ForceMode.Acceleration);
            }


            Rotaion(hit, rayDistance);


            //si esta por debajo de la altura que queremos y rb.velocity.y<0  si esta por encima de la altura que queremos y rb.velocity.y>0 reducimos la velocidad del eje y
            if ((diference > 0 && rb.velocity.y > 0) || (diference < 0 && rb.velocity.y < 0))
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.9f, rb.velocity.z);
            }

            if (!Physics.Raycast(ray, out hit, levitationHeight * 2, LayerMask.GetMask("Floor")))
            {
                rb.AddForce(-Vector3.up * extraFallImpulse * Time.fixedDeltaTime, ForceMode.VelocityChange);

            }
        }

    }

    //Modificamos la rotación de la nave
    private void Rotaion(RaycastHit hit, float rayDistance)
    {

        //hay que corregir que no se suba por cualquier superficie

        if (rayDistance > startCorrectionHeight)
        {
            naveManager.isPlanning = true;
            animationManager.plane = true;
            Quaternion interpolation;
            interpolation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, transform.localEulerAngles.y, 0), Time.fixedDeltaTime * upDamping);
            transform.localRotation = interpolation;
        }
        else
        {
            naveManager.isPlanning = false;
            animationManager.plane = false;
            Quaternion rot = transform.localRotation;
            Quaternion quaternionRot = transform.localRotation; //guardamos la rotación actual en quaternions

            transform.up = hit.normal; //hacemos que la nave este perpendicular a la normal del punto donde ha colisionado el raycast

            Quaternion quatNewRot = transform.localRotation;  //guardamos la rotación en quaternions despues de corregirla 
            Quaternion interpolation;
            //hacemos una interpolación entre la rotación inicial y la final en relación a la distancia al suelo

            //interpolation = Quaternion.Lerp(quaternionRot, quatNewRot, (1 - ((rayDistance - levitationHeight) / startCorrectionHeight)) * (1 / rayDistance));
            interpolation = Quaternion.Lerp(quaternionRot, quatNewRot, (rayDistance - levitationHeight) < levitationHeight * 0.2f ? Time.fixedDeltaTime * damping : (1 - ((rayDistance - levitationHeight) / startCorrectionHeight)) * (1 / rayDistance));

            //igualamos la rotación a el resultado de la interpolación
            transform.localRotation = interpolation;

            /*if(GetComponent<NaveController>().modelTransform.forward.y<lastInclination && rb.velocity.y>0)
            {
                if(rayDistance>levitationHeight)
                {
                    transform.localRotation = rot;
                }
            }*/
        }
    }
}
