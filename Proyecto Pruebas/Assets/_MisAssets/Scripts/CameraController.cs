using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : Photon.PunBehaviour
{


    public Vector2 sensitivity;

    public Transform target, frontLookAt, backLookAt;
    [Range(1f, 10f)]
    public float damping = 4f;
    public float cameraDampingMultiplayer = 1;

    public GameObject gameOverCamera;
    public GameObject winnerCamera;
    public GameObject finishCamera;
    public Sprite singleplayerGameOverSprite;
    public LayerMask ignoreLayers;


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
        localPos = transform.parent.localPosition;
        if (!photonView.isMine) transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        string[] joysticks = Input.GetJoystickNames();
        for (int i = 0; i < joysticks.Length; i++)
        {
            print("joystick [" + i + "] " + joysticks[i]);
        }
        if (naveDestruida) return;

        //hacemos que la posición del padre sea igual a la de la nave
        transform.parent.parent.parent.position = target.position;
        //hacemos que la rotacion del pivote sea una interpolación entre su rotación y la de la nave respecto a Time.deltaTime
        transform.parent.parent.rotation = Quaternion.Lerp(transform.parent.parent.rotation, target.rotation, Time.deltaTime * damping * cameraDampingMultiplayer);

        //transform.localPosition = transform.parent.position - diference + velocityOffset;
        //Vector3 rot = transform.rotation.eulerAngles;
        //transform.rotation = Quaternion.Euler(new Vector3(rot.x, target.rotation.eulerAngles.y, rot.z));

        //cambiamos de cámara al pulsar un botón
        if (inputManager.ChangeCamera())
        {
            backCamera = !backCamera;
        }

        CameraFocus(!backCamera);
        PositionCamera();

    }

    private void PositionCamera()
    {
        Vector3 camPos = transform.parent.position;
        Vector3 camCentre = transform.parent.parent.position;

        RaycastHit hit = new RaycastHit();

        if (Physics.Linecast(camCentre, camPos, out hit, ignoreLayers))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = camPos;
        }
    }

    public void OnShipDestroyed(float time)
    {
        transform.SetParent(null);
        StartCoroutine(GameOver(time));
    }

    public void OnRaceFinished(int place)
    {
        print("Place: " + place);
        transform.SetParent(null);
        GameObject finishCam = Instantiate(place==1? winnerCamera : finishCamera, transform.position, transform.rotation);

        finishCam.GetComponent<Camera>().fieldOfView = GetComponent<Camera>().fieldOfView;

        FinishCamera fc =finishCam.GetComponent<FinishCamera>();

        fc.placeText.text = place.ToString();

        if (String.Equals(fc.placeText.text, "1") || String.Equals(fc.placeText.text, "21") || String.Equals(fc.placeText.text, "31") || String.Equals(fc.placeText.text, "41") || String.Equals(fc.placeText.text, "51") || String.Equals(fc.placeText.text, "61") || String.Equals(fc.placeText.text, "71") || String.Equals(fc.placeText.text, "81") || String.Equals(fc.placeText.text, "91"))
        {
            fc.placeSufijoText.text = "st";
        }
        else if (String.Equals(fc.placeText.text, "2") || String.Equals(fc.placeText.text, "22") || String.Equals(fc.placeText.text, "32") || String.Equals(fc.placeText.text, "42") || String.Equals(fc.placeText.text, "52") || String.Equals(fc.placeText.text, "62") || String.Equals(fc.placeText.text, "72") || String.Equals(fc.placeText.text, "82") || String.Equals(fc.placeText.text, "92"))
        {
            fc.placeSufijoText.text = "nd";
        }
        else if (String.Equals(fc.placeText.text, "3") || String.Equals(fc.placeText.text, "23") || String.Equals(fc.placeText.text, "33") || String.Equals(fc.placeText.text, "43") || String.Equals(fc.placeText.text, "53") || String.Equals(fc.placeText.text, "63") || String.Equals(fc.placeText.text, "73") || String.Equals(fc.placeText.text, "83") || String.Equals(fc.placeText.text, "93"))
        {
            fc.placeSufijoText.text = "rd";
        }
        else
        {
            fc.placeSufijoText.text = "th";
        }

        fc.timeText.text = String.Format("{0:00}'{1:00}''", TimeScore.currentScore.minutes, TimeScore.currentScore.seconds);

        Destroy(gameObject);
    }

    public IEnumerator GameOver(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject gameOverCam = Instantiate(gameOverCamera,transform.position, transform.rotation);

        gameOverCam.GetComponent<Camera>().fieldOfView = GetComponent<Camera>().fieldOfView;
        //if (Global.numPlayers == 1)
        //{
        //    gameOverCam.GetComponent<Camera>().rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
        //    gameOverCam.GetComponentInChildren<Image>().sprite = singleplayerGameOverSprite;
        //}
        Destroy(gameObject);
    }

    private void CameraFocus(bool front)
    {
        //transform.parent.parent.rotation = target.rotation;


        transform.parent.parent.parent.rotation = Quaternion.Lerp(transform.parent.parent.parent.rotation, target.rotation, Time.deltaTime * damping);

        transform.parent.parent.parent.localRotation = Quaternion.Euler(transform.parent.parent.parent.localEulerAngles.x, transform.parent.parent.parent.localEulerAngles.y, 0);

        //transform.parent.parent.position = Vector3.Lerp(transform.parent.parent.position,target.position, Time.deltaTime * damping);


        //cambiamos el valor de currentx y currenty respecto a el desplazamiento del joystick derecho/ ratón
        if (!PauseManager.inPause)
        {
            if (front)
            {
                currentX += inputManager.CameraHorizontal() * sensitivity.x;
                currentY += inputManager.CameraVertical() * sensitivity.y * (OptionsMenu.inverted/*(PauseManager.invertY[inputManager.numPlayer - 1]*/ ? 1 : -1);
            }
            else
            {
                currentX += inputManager.CameraHorizontal() * sensitivity.x;
                currentY -= inputManager.CameraVertical() * sensitivity.y * (PauseManager.invertY[inputManager.numPlayer - 1] ? 1 : -1);
            }
        }


        //vuelve a apuntar al centro si se pulsa el botón
        if (inputManager.ResetCamera())
        {
            StartCoroutine(ResetCameraFocus());
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
            transform.parent.localPosition = localPos;
        }
        else
        {
            transform.parent.localPosition = new Vector3(localPos.x, localPos.y, -localPos.z);
        }



        /*float z = transform.localEulerAngles.z; //guardamos la rotación local z

        Quaternion oldRot = transform.rotation; //guardamos la rotación antes de apuntar al objetivo*/

        transform.parent.parent.localRotation = Quaternion.Euler(currentY, currentX, 0);
        transform.parent.parent.rotation = Quaternion.Euler(transform.parent.parent.eulerAngles.x, transform.parent.parent.eulerAngles.y, 0);

        if (front)
        {
            //hacemos que la camara apunte a un objeto que tenemos delante de la nave
            transform.parent.LookAt(frontLookAt);
        }
        else
        {
            //hacemos que la camara apunte a un objeto que tenemos detras de la nave
            transform.parent.LookAt(backLookAt);
        }


        /*
                float newZ = transform.localEulerAngles.z;  //guardamos la nueva rotación local z

                transform.Rotate(new Vector3(0, 0, z - newZ));  //

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, z);

                Quaternion newRot = transform.rotation;

                transform.rotation = Quaternion.Lerp(oldRot, newRot, Time.deltaTime * damping * cameraDampingMultiplayer);*/
    }

    public IEnumerator ResetCameraFocus()
    {
        float _currentY = currentY;
        float _currentX = currentX;
        int loops = 5;
        float loop = 1f / (float)loops;
        for (int i = 1; i <= loops; i++)
        {
            float t = loop * (float)i;
            currentX = Mathf.Lerp(_currentX, 0, t);
            currentY = Mathf.Lerp(_currentY, 0, t);
            yield return new WaitForEndOfFrame();
        }
    }

    

}
