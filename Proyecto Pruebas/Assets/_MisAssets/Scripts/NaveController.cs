using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveController : MonoBehaviour
{
    [Tooltip("Pon la cámara de la nave")]
    public Camera myCamera;   //cámara de la nave
    [Tooltip("Pon la altura máxima a la que la nave podrá controlarse, a partir de esta altura los controles no funcionarán")]
    public float maneuverHeight;    //altura a la que se puede manejar la nave
    [Tooltip("Pon la fricción de la nave")]
    public float friction;      //la fricción de la nave, ya que esta flotando y no roza con nada hay que aplicarle una fricción para que no se deslice hasta el infinito
    [Tooltip("Pon la velocidad lateral de la nave al derrapar")]
    public float driftVelocity;     //velocidad lateral que se aplica cuando la nave derrapa
    [Tooltip("Pon la altura a la que la nave flotará")]
    public float levitationHeight;  //altura a la que queremos que la nave flote
    [Tooltip("Pon el modificador a la velocidad cuando va marcha atrás, 1 es igual, 0,7 un 70%, 1,3 un 130%")]
    public float backwardVelocity;  //modificador de velocidad cuando la nave va marcha atrás, se multiplica por la velocidad
    [Tooltip("Pon el impulso vertical extra que se le aplica a la nave para que no parezca que cae a cámara lenta")]
    public float extraFallImpulse;  //impulso extra que se le aplica a la nave al caer para que no parezca que cae a cámara lenta

    [Header("Constantes fórmulas")]
    [Tooltip("Pon la constante de la vida")]
    public float healthConst;   //constante que se le multiplica a la vida en la fórmula de velocidad
    [Tooltip("Pon la constante de la posición")]
    public float positionConst; //constante que se le multiplica a la posición en la fórmula de velocidad
    [Tooltip("Pon la constante del rebufo")]
    public float recoilConst;   //constante que se le multiplica al rebufo(recoil) en la fórmula de velocidad
    [Tooltip("Pon la constante del turbo")]
    public float turboConst;    //constante que se le multiplica al turbo en la fórmula de velocidad

    [Header("Piezas de la nave")]
    [Tooltip("Pon el transform del objeto que contiene las diferentes piezas de la nave")]
    public Transform modelTransform;  //transform del objeto que contiene las piezas de la nave
    [Tooltip("Asigna las piezas de la nave")]
    public List<Pieza> piezas = new List<Pieza>(); //lista con todas las piezas de la nave
    [Tooltip("Asigna la pieza que sea el núcleo de la nave")]
    public Pieza nucleo;  //Variable que contiene la pieza que es el núcleo de la nave

    private bool inRecoil = false;  //variable que controla cuando la nave esta cogiendo rebufo
    private bool inBoost = false;   //variable que controla cuando la nave esta en turbo
    private bool inDrift = false;   //variable que controla cuando esta derrapando la nave

    private Rigidbody rb;   //rigidbody de la nave
    private float position = 0;     //variable que indica la posición de la nave en la carrera, sirve para hacer cálculos de velocidad

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Controller();
    }

    public void Controller()
    {

        Camera.SetupCurrent(myCamera);

        //convertimos la velocidad de global a local
        Vector3 locVel = modelTransform.InverseTransformDirection(rb.velocity);

        //depende de la velocidad la camara esta mas o menos cerca del coche
        myCamera.gameObject.GetComponent<CameraController>().velocityOffset = new Vector3(0, 0, Mathf.Clamp(locVel.z / (GetComponent<Maneuverability>().currentVelocity / 15), -4f, 5f));
        myCamera.fieldOfView = 60f + Mathf.Clamp(locVel.z / 15f, 0f, 80f);



        if (!inDrift)
        {
            //disminuimos poco a poco la velocidad lateral para que no se vaya demasiado la nave
            locVel.x *= 0.95f;
            if (Mathf.Abs(locVel.x) < 2f)
            {
                locVel = new Vector3(0, locVel.y, locVel.z);
            }
        }


        Ray ray = new Ray();
        RaycastHit hit;

        ray.origin = transform.position;//+ new Vector3(0, 0, Mathf.Clamp(locVel.z / (velocity / 10), -6f, 6f));
        ray.direction = -Vector3.up;


        //si el vehiculo esta cerca del suelo 
        if (Physics.Raycast(ray, out hit, maneuverHeight, LayerMask.GetMask("Floor")))
        {

            //mover hacia adelante
            if (Input.GetAxis("Nave Vertical") > 0)
            {
                if (locVel.z < 0) // si estas moviendote hacia atras y quieres ir hacia adelante se ayuda a parar el vehiculo
                {
                    //rb.velocity = new Vector3(rb.velocity.x * (1 - friction*2), rb.velocity.y, rb.velocity.z * (1 - friction*2));
                    locVel = new Vector3(locVel.x, locVel.y, locVel.z * (1 - (friction)));
                }
                if (inDrift)
                {
                    if (Input.GetAxis("Horizontal") > 0)
                    {
                        if (locVel.x > 0)
                        {
                            locVel.x = 0;
                        }
                        rb.AddForce(modelTransform.forward * 0.2f * Input.GetAxis("Nave Vertical") * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                        rb.AddForce(-modelTransform.right * driftVelocity * Input.GetAxis("Nave Vertical") * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                    }
                    else if (Input.GetAxis("Horizontal") < 0)
                    {
                        if (locVel.x < 0)
                        {
                            locVel.x = 0;
                        }
                        rb.AddForce(modelTransform.forward * 0.2f * Input.GetAxis("Nave Vertical") * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                        rb.AddForce(modelTransform.right * driftVelocity * Input.GetAxis("Nave Vertical") * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                    }
                    else
                    {
                        //rb.AddForce(transform.forward * Input.GetAxis("Nave Vertical") * Mathf.Pow(aceleration, 2) * Time.deltaTime, ForceMode.Impulse);
                    }

                }
                else
                {
                    rb.AddForce((modelTransform.forward * Input.GetAxis("Nave Vertical") * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime), ForceMode.VelocityChange); //fuerza para moverte hacia adelante
                }



            }
            else if (Input.GetAxis("Nave Vertical") < 0)  //mover hacia atras
            {
                if (locVel.z > 0)// si estas moviendote hacia adelante y quieres ir hacia atras se ayuda a parar el vehiculo
                {
                    locVel = new Vector3(locVel.x, locVel.y, locVel.z * (1 - (friction)));
                    //rb.velocity = new Vector3(rb.velocity.x * (1 - friction*2), rb.velocity.y, rb.velocity.z * (1 - friction*2));
                }
                rb.AddForce(modelTransform.forward * Input.GetAxis("Nave Vertical") * Mathf.Pow(GetComponent<Maneuverability>().acceleration, 2) * backwardVelocity * Time.deltaTime, ForceMode.Impulse); // fuerza para moverte hacia atras


            }



            //rotación al girar

            if (Input.GetAxis("Nave Vertical") >= 0)
            {
                modelTransform.localRotation = Quaternion.Euler(modelTransform.localRotation.eulerAngles.x, modelTransform.localRotation.eulerAngles.y + (Input.GetAxis("Horizontal") * GetComponent<Maneuverability>().maneuver * Time.deltaTime), modelTransform.localRotation.eulerAngles.z);
            }
            else if (Input.GetAxis("Nave Vertical") < 0)
            {
                modelTransform.localRotation = Quaternion.Euler(modelTransform.localRotation.eulerAngles.x, modelTransform.localRotation.eulerAngles.y - (Input.GetAxis("Horizontal") * GetComponent<Maneuverability>().maneuver * Time.deltaTime), modelTransform.localRotation.eulerAngles.z);
            }


            //derrape
            if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.LeftShift))
            {
                if (locVel.z > 0)
                {
                    inDrift = true;
                    myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = 0.3f;
                    //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (Input.GetAxis("Horizontal") * getComponent<Maneuverability>().maneuver * Time.deltaTime * 2), transform.rotation.eulerAngles.z);
                }
                else
                {
                    myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = 1f;
                    inDrift = false;
                }
            }
            else
            {
                myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = 1f;
                inDrift = false;
            }


            //si no se estan pulsando las teclas que hacen moverse al vehiculo
            if (!AnyMovementKeys)
            {
                //locVel = new Vector3(locVel.x, locVel.y, locVel.z * (1 - (friction))); //se ralentiza el vehiculo
                locVel = new Vector3(locVel.x, locVel.y, locVel.z - locVel.z * 0.02f);
                if (Mathf.Abs(locVel.z) < 2f)
                {
                    locVel = new Vector3(locVel.x, locVel.y, 0f);
                }
            }

        }
        else //si el vehiculo no esta cerca del suelo se añade una fuerza para que caiga más rapido (de lo contrario tarda mucho en caer)
        {
            myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = 1f;
            inDrift = false;



        }



        if (!Physics.Raycast(ray, out hit, levitationHeight * 2, LayerMask.GetMask("Floor")))
        {
            //Vector3 rayDistance = ray.origin - hit.point; //guardamos la distancia del raycast
            rb.AddForce(-Vector3.up * extraFallImpulse, ForceMode.VelocityChange);
            //if(rayDistance.magnitude/2<levitationHeight)
            //{
            //    startCorrectionHeight = saveStartCorrectingHeight;
            //}

        }




        rb.angularVelocity = Vector3.zero;

        //rotación lateral al girar
        //piezasGameObject.localEulerAngles = new Vector3(piezasGameObject.localEulerAngles.x, piezasGameObject.localEulerAngles.y, Mathf.LerpAngle(piezasGameObject.localEulerAngles.z, Mathf.Clamp(maxInclination * -Input.GetAxis("Horizontal") * (rb.velocity.magnitude / MaxVelocity) * (maniobrabilidad / 100), -maxInclination, maxInclination), Time.deltaTime * rotationDamping));

        //si no se esta girando hece que el vehiculo deje de rotar
        if (Input.GetAxis("Horizontal") == 0)
        {
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, rb.angularVelocity.z);
        }

        //rb.angularVelocity = new Vector3(rb.angularVelocity.x, rb.angularVelocity.y*0.95f, rb.angularVelocity.z);

        //controlamos la velocidad no vertical para ponerle un tope
        Vector2 notVerticalVel = new Vector2(locVel.x, locVel.z);

        //si la velocidad no vertical supera la velocidad maxima del vehiculo la bajamos hasta la velocidad maxima
        if (notVerticalVel.magnitude > GetComponent<Maneuverability>().MaxVelocity)
        {
            Vector2 correctedVel = notVerticalVel.normalized * GetComponent<Maneuverability>().MaxVelocity;

            locVel = new Vector3(correctedVel.x, locVel.y, correctedVel.y);
        }

        //convertimos la velocidad local en la velocidad global y la aplicamos
        rb.velocity = modelTransform.TransformDirection(locVel);



    }

    public bool AnyMovementKeys
    {
        get { return (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Joystick1Button2) || Input.GetAxis("Nave Vertical") != 0)/* || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)*/; }
    }

    public float VelocityFormula    //devuelve la velocidad máxima de la nave aplicando todos los modificadores
    {
        get { return GetComponent<Maneuverability>().MaxVelocity + (PorcentajeSalud * healthConst) + (DistanciaPrimero * positionConst) + ((recoilConst * (inRecoil ? 1 : 0)) * GetComponent<Maneuverability>().currentRecoil) + ((turboConst * (inBoost ? 1 : 0)) * GetComponent<Maneuverability>().currentTurbo); }
    }

    public float PorcentajeSalud    //devuelve el porcentaje de salud de la nave
    {
        get { return (nucleo.currentHealth / GetComponent<Stats>().life) * 100; }
    }

    public float DistanciaPrimero   //devuelve la distancia de la nave respecto al primero de la carrera
    {
        get { return 1; }
    }
}
