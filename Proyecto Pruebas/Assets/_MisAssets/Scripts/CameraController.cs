using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : Photon.PunBehaviour
{


    public Vector2 sensibility;

    public Transform target, frontLookAt, backLookAt;
    [Range(1f, 10f)]
    public float damping = 4f;
    public float cameraDampingMultiplayer = 1;

    public GameObject gameOverCamera;
    public Sprite singleplayerGameOverSprite;


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
    
    private float currentX = 0.0f;
    private float currentY = 45.0f;
    public Vector3 localPos { get; set; }
    private bool backCamera = false;


    public Vector3 velocityOffset { get; set; }

    public InputManager inputManager;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        naveDestruida = false;
        //diference = transform.parent.position - transform.position;

        currentX = 0;
        currentY = 0;

        //guardamos la posición inicial 
        localPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (naveDestruida || PauseManager.inPause) return;

        //hacemos que la posición del padre sea igual a la de la nave
        transform.parent.parent.position = target.position;
        //hacemos que la rotacion del pivote sea una interpolación entre su rotación y la de la nave respecto a Time.deltaTime
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, target.rotation, Time.deltaTime * damping * cameraDampingMultiplayer);

        //transform.localPosition = transform.parent.position - diference + velocityOffset;
        //Vector3 rot = transform.rotation.eulerAngles;
        //transform.rotation = Quaternion.Euler(new Vector3(rot.x, target.rotation.eulerAngles.y, rot.z));

        //cambiamos de cámara al pulsar un botón
        if (inputManager.ChangeCamera())
        {
            backCamera = !backCamera;
        }

        CameraFocus(!backCamera);
        
    }

    public void OnShipDestroyed(float time)
    {
        transform.SetParent(null);
        StartCoroutine(GameOver(time));
    }
    
    public IEnumerator GameOver(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject gameOverCam=Instantiate(gameOverCamera, new Vector3(0, -500, 0), Quaternion.identity);
        if(Global.numPlayers==1)
        {
            gameOverCam.GetComponent<Camera>().rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
            gameOverCam.GetComponentInChildren<Image>().sprite = singleplayerGameOverSprite;
        }
        Destroy(gameObject);
    }

    private void CameraFocus(bool front)
    {
        //transform.parent.parent.rotation = target.rotation;

        transform.parent.parent.rotation = Quaternion.Lerp(transform.parent.parent.rotation, target.rotation, Time.deltaTime * damping);

        transform.parent.parent.localRotation = Quaternion.Euler(transform.parent.parent.localEulerAngles.x, transform.parent.parent.localEulerAngles.y, 0);

        //transform.parent.parent.position = Vector3.Lerp(transform.parent.parent.position,target.position, Time.deltaTime * damping);


        //cambiamos el valor de currentx y currenty respecto a el desplazamiento del joystick derecho/ ratón
        if(front)
        {
            currentX += inputManager.CameraHorizontal() * sensibility.x;
            currentY += inputManager.CameraVertical() * sensibility.y * (PauseManager.invertY[inputManager.numPlayer - 1] ? 1 : -1);
        }
        else
        {
            currentX -= inputManager.CameraHorizontal() * sensibility.x;
            currentY += inputManager.CameraVertical() * sensibility.y * (PauseManager.invertY[inputManager.numPlayer - 1] ? 1 : -1);
        }
        

        
        //vuelve a apuntar al centro si se pulsa el botón
        if(inputManager.ResetCamera())
        {
            currentX *= 0.9f;
            currentY *= 0.9f;
        }


        /*
        if (inputManager.CameraHorizontal() == 0)         //
        {                                           //
            currentX *= 0.9f;                       //
        }                                           // para mando solo
        if (inputManager.CameraVertical() == 0)         //
        {                                           //
            currentY *= 0.9f;                       //
        }    */                                       //
        

        
        //limitamos la posición de la camara
        currentY = Mathf.Clamp(currentY, min_Y_Angle, max_Y_Angle);
        currentX = Mathf.Clamp(currentX, min_X_Angle, max_X_Angle);



        //igualamos la posicion de la camara a la posición inicial + el desplazamiento de camara + el desplazamiento por velocidad
        if (front)
        {
            transform.localPosition = localPos;
        }
        else
        {
            transform.localPosition = new Vector3(localPos.x, localPos.y, -localPos.z) ;
        }



        /*float z = transform.localEulerAngles.z; //guardamos la rotación local z

        Quaternion oldRot = transform.rotation; //guardamos la rotación antes de apuntar al objetivo*/

        transform.parent.localRotation = Quaternion.Euler(currentY, currentX, 0);
        transform.parent.rotation = Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0);

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
