using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Photon.PunBehaviour
{

    private const float Y_ANGLE_MIN = -10.0f;
    private const float Y_ANGLE_MAX = 30.0f;


    public Vector2 sensibility;

    public Transform target, frontLookAt, backLookAt;
    public float distance = 10;
    [Range(1f, 10f)]
    public float damping = 4f;
    public float cameraDampingMultiplayer = 1;

    [Header("Camera Limits")]
    [Range(0f, 200f)]
    public float max_X_Angle = 30f;
    [Range(0f, -200f)]
    public float min_X_Angle = 30f;
    [Range(0f, 200f)]
    public float max_Y_Angle = 30f;
    [Range(0f, -200f)]
    public float min_Y_Angle = -10f;

    public bool naveDestruida { get; set; }

    private Vector3 diference;
    private float currentX = 0.0f;
    private float currentY = 45.0f;
    public Vector3 localPos { get; set; }
    private bool backCamera = false;


    public Vector3 velocityOffset { get; set; }

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        naveDestruida = false;
        //diference = transform.parent.position - transform.position;


        //guardamos la posición inicial 
        localPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (naveDestruida) return;

        //hacemos que la posición del padre sea igual a la de la nave
        transform.parent.position = target.position;
        //hacemos que la rotacion del pivote sea una interpolación entre su rotación y la de la nave respecto a Time.deltaTime
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, target.rotation, Time.deltaTime * damping * cameraDampingMultiplayer);

        //transform.localPosition = transform.parent.position - diference + velocityOffset;
        //Vector3 rot = transform.rotation.eulerAngles;
        //transform.rotation = Quaternion.Euler(new Vector3(rot.x, target.rotation.eulerAngles.y, rot.z));

        //cambiamos de cámara al pulsar un botón
        if (Input.GetKeyDown(KeyCode.Joystick1Button11))
        {
            backCamera = !backCamera;
        }

        CameraFocus(!backCamera);
        
    }

    private void CameraFocus(bool front)
    {
        //cambiamos el valor de currentx y currenty respecto a el desplazamiento del joystick derecho/ ratón
        if(front)
        {
            currentX += Input.GetAxis("Camera X") * sensibility.x;
            currentY += Input.GetAxis("Camera Y") * sensibility.y;
        }
        else
        {
            currentX -= Input.GetAxis("Camera X") * sensibility.x;
            currentY += Input.GetAxis("Camera Y") * sensibility.y;
        }
        

        
        //vuelve a apuntar al centro si se suelta el joystick
        if (Input.GetAxis("Camera X") == 0)         //
        {                                           //
            currentX *= 0.9f;                       //
        }                                           // para mando solo
        if (Input.GetAxis("Camera Y") == 0)         //
        {                                           //
            currentY *= 0.9f;                       //
        }                                           //
        

        
        //limitamos la posición de la camara
        currentY = Mathf.Clamp(currentY, min_Y_Angle, max_Y_Angle);
        currentX = Mathf.Clamp(currentX, min_X_Angle, max_X_Angle);
        

        
        //igualamos la posicion de la camara a la posición inicial + el desplazamiento de camara + el desplazamiento por velocidad
        if (front)
        {
            transform.localPosition = localPos + new Vector3(-currentX, currentY, 0) / 10; /*- velocityOffset;*/
        }
        else
        {
            transform.localPosition = new Vector3(localPos.x, localPos.y, -localPos.z) + new Vector3(-currentX, currentY, 0) / 10;
        }
        
        

        float z = transform.localEulerAngles.z; //guardamos la rotación local z

        Quaternion oldRot = transform.rotation; //guardamos la rotación antes de apuntar al objetivo



        if(front)
        {
            //hacemos que la camara apunte a un objeto que tenemos delante de la nave
            transform.LookAt(frontLookAt);
        }
        else
        {
            //hacemos que la camara apunte a un objeto que tenemos detras de la nave
            transform.LookAt(backLookAt);
        }
        

/*
        float newZ = transform.localEulerAngles.z;  //guardamos la nueva rotación local z

        transform.Rotate(new Vector3(0, 0, z - newZ));  //

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, z);

        Quaternion newRot = transform.rotation;

        transform.rotation = Quaternion.Lerp(oldRot, newRot, Time.deltaTime * damping * cameraDampingMultiplayer);*/
    }
}
