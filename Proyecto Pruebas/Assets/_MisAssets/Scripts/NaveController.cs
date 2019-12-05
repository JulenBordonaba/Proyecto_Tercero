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

    [Header("Constantes fórmulas")]
    [Tooltip("Pon la velocidad máxima que se aumenta según la vida, se multiplica con el porcentaje de vida de la nave")]
    public float healthConst;   //constante que se le multiplica a la vida en la fórmula de velocidad
    [Tooltip("Pon la velocidad máxima que se aumenta según la posición, se multiplica con la posición de la nave empezando desde detrás")]
    public float positionConst; //constante que se le multiplica a la posición en la fórmula de velocidad
    [Tooltip("Pon la velocidad máxima que se aumenta cuando se está en rebufo")]
    public float recoilConst;   //constante que se le multiplica al rebufo(recoil) en la fórmula de velocidad
    [Tooltip("Pon la velocidad máxima que se aumenta cuando se está en turbo, se multiplica con la stat de turbo actual de la nave")]
    public float turboConst;    //constante que se le multiplica al turbo en la fórmula de velocidad

    [Header("Piezas de la nave")]
    [Tooltip("Pon el transform del objeto que contiene las diferentes piezas de la nave")]
    public Transform modelTransform;  //transform del objeto que contiene las piezas de la nave
    [Tooltip("Asigna las piezas de la nave")]
    public List<Pieza> piezas = new List<Pieza>(); //lista con todas las piezas de la nave
    [Tooltip("Asigna la pieza que sea el núcleo de la nave")]
    public Pieza nucleo;  //Variable que contiene la pieza que es el núcleo de la nave
    [Tooltip("Pon los grados máximos que se puede inclinar la nave al girar")]
    public float maxInclination = 30;
    [Tooltip("pon el tiempo que tarda la nave en corregir su rotación al girar cuando está lejos del suelo")]
    public float rotationDamping = 2;
    [Tooltip("Pon el field of view base de la cámara")]
    public float fieldOfView = 60f;
    [Tooltip("Pon la inclinación máxima por la que puede escalar la nave, rango 0-1")]
    [Range(0f, 1f)]
    public float maxSubida;
    /*[Tooltip("Pon el desfase mínimo de la cámara")]
    public float minCameraOffset = -4;  
    [Tooltip("Pon el desfase máximo de la cámara")]
    public float maxCameraOffset = 5;*/

    private bool inRecoil = false;  //variable que controla cuando la nave esta cogiendo rebufo
    public bool inBoost { get; set; }   //variable que controla cuando la nave esta en turbo
    public bool inDrift = false;   //variable que controla cuando esta derrapando la nave

    private Rigidbody rb;   //rigidbody de la nave
    private float position = 0;     //variable que indica la posición de la nave en la carrera, sirve para hacer cálculos de velocidad
    private InputManager inputManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (PauseManager.inPause) return;
        
        if (inBoost)
        {
            ApplyTurbo();
        }
        if (GetComponent<NaveManager>().isPlanning) return;
        Controller();

    }
    

    public void Controller()
    {

        Camera.SetupCurrent(myCamera);

        //convertimos la velocidad de global a local
        Vector3 locVel = modelTransform.InverseTransformDirection(rb.velocity);
        GetComponent<NaveAnimationManager>().move = locVel.z != 0;
        //depende de la velocidad la camara esta mas o menos cerca del coche
        //myCamera.gameObject.GetComponent<CameraController>().velocityOffset = new Vector3(0, 0, Mathf.Clamp(locVel.z / (GetComponent<Maneuverability>().currentVelocity / 15), minCameraOffset, maxCameraOffset));
        myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, fieldOfView + Mathf.Clamp(locVel.z * (inBoost ? 2 : 1) / 15f, 0f, 80f), Time.deltaTime);



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
            if (inputManager.Accelerate() > 0)
            {
                if (locVel.z < 0) // si estas moviendote hacia atras y quieres ir hacia adelante se ayuda a parar el vehiculo
                {
                    locVel = new Vector3(locVel.x, locVel.y, locVel.z * (1 - (friction)));
                }
                if (inDrift)    //si la nave esta derrapando
                {
                    if (inputManager.MainHorizontal() > 0)    //si esta girando hacia la derecha
                    {
                        if (locVel.x > 0)   //si la velocidad lateral hacia la derecha es positiva se pone a 0
                        {
                            locVel.x = 0;
                        }
                        //impulso hacia delante, más pequeño que cuando no esta derrapando
                        rb.AddForce(modelTransform.forward * 0.2f * inputManager.Accelerate() * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                        //impulso lateral hacia el lado contrario que se esta girando
                        Quaternion rot = modelTransform.rotation;
                        modelTransform.rotation = Quaternion.Euler(modelTransform.eulerAngles.x, modelTransform.eulerAngles.y, 0);
                        rb.AddForce(-modelTransform.right * driftVelocity * inputManager.Accelerate() * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                        modelTransform.rotation = rot;
                    }
                    else if (inputManager.MainHorizontal() < 0)   //si esta girando hacia la izquierda
                    {
                        if (locVel.x < 0)
                        {
                            locVel.x = 0;
                        }
                        //impulso hacia delante, más pequeño que cuando no esta derrapando
                        rb.AddForce(modelTransform.forward * 0.2f * inputManager.Accelerate() * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                        //impulso lateral hacia el lado contrario que se esta girando
                        Quaternion rot = modelTransform.rotation;
                        modelTransform.rotation = Quaternion.Euler(modelTransform.eulerAngles.x, modelTransform.eulerAngles.y, 0);
                        rb.AddForce(modelTransform.right * driftVelocity * inputManager.Accelerate() * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                        modelTransform.rotation = rot;
                    }
                    else
                    {
                        //rb.AddForce(transform.forward * Input.GetAxis("Nave Vertical") * Mathf.Pow(aceleration, 2) * Time.deltaTime, ForceMode.Impulse);
                    }

                }
                else
                {

                    rb.AddForce(modelTransform.forward * inputManager.Accelerate() * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange); //fuerza para moverte hacia adelante
                }



            }
            else if (inputManager.Accelerate() < -0.5f)  //mover hacia atras
            {
                if (locVel.z > 0)// si estas moviendote hacia adelante y quieres ir hacia atras se ayuda a parar el vehiculo
                {
                    locVel = new Vector3(locVel.x, locVel.y, locVel.z * (1 - (friction)));
                }
                rb.AddForce(modelTransform.forward * inputManager.Accelerate() * GetComponent<Maneuverability>().AcelerationWithWeight * backwardVelocity * Time.deltaTime, ForceMode.VelocityChange); // fuerza para moverte hacia atras

            }



            //rotación al girar

            if (locVel.z >= -0.2f)
            {
                modelTransform.localRotation = Quaternion.Euler(modelTransform.localRotation.eulerAngles.x, modelTransform.localRotation.eulerAngles.y + (inputManager.MainHorizontal() * GetComponent<Maneuverability>().currentManeuver * Time.deltaTime), modelTransform.localRotation.eulerAngles.z);
            }
            else if (locVel.z < -0.2f)
            {
                modelTransform.localRotation = Quaternion.Euler(modelTransform.localRotation.eulerAngles.x, modelTransform.localRotation.eulerAngles.y - (inputManager.MainHorizontal() * GetComponent<Maneuverability>().currentManeuver * Time.deltaTime), modelTransform.localRotation.eulerAngles.z);
            }


            //derrape
            if (inputManager.Drift())
            {
                if (locVel.z > 0)
                {
                    inDrift = true;
                    myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = 0.3f;
                }
                else
                {
                    myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = Mathf.Lerp(myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer, 1f, Time.deltaTime);
                    inDrift = false;
                }
            }
            else
            {
                myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = Mathf.Lerp(myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer, 1f, Time.deltaTime);
                inDrift = false;
            }


            //si no se estan pulsando las teclas que hacen moverse al vehiculo
            if (!AnyMovementKeys)
            {
                locVel = new Vector3(locVel.x, locVel.y, locVel.z * (1 - (friction))); //se ralentiza el vehiculo
                //locVel = new Vector3(locVel.x, locVel.y, locVel.z - locVel.z * 0.02f);
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








        rb.angularVelocity = Vector3.zero;

        //rotación lateral al girar
        modelTransform.localEulerAngles = new Vector3(modelTransform.localEulerAngles.x, modelTransform.localEulerAngles.y, Mathf.LerpAngle(modelTransform.localEulerAngles.z, Mathf.Clamp(maxInclination * -inputManager.MainHorizontal() * (rb.velocity.magnitude / VelocityFormula) * (GetComponent<Maneuverability>().currentManeuver / 100), -maxInclination, maxInclination), Time.deltaTime * rotationDamping));

        //si no se esta girando hece que el vehiculo deje de rotar
        if (inputManager.MainHorizontal() == 0)
        {
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, rb.angularVelocity.z);
        }

        //rb.angularVelocity = new Vector3(rb.angularVelocity.x, rb.angularVelocity.y*0.95f, rb.angularVelocity.z);

        //controlamos la velocidad no vertical para ponerle un tope
        Vector2 notVerticalVel = new Vector2(locVel.x, locVel.z);

        //si la velocidad no vertical supera la velocidad maxima del vehiculo la bajamos hasta la velocidad maxima
        if (notVerticalVel.magnitude > VelocityFormula)
        {
            Vector2 correctedVel = notVerticalVel.normalized * VelocityFormula;

            //locVel = Vector3.Lerp( locVel,new Vector3(correctedVel.x, locVel.y, correctedVel.y),Time.deltaTime*1000);
            locVel = new Vector3(correctedVel.x, locVel.y, correctedVel.y);
        }


        //convertimos la velocidad local en la velocidad global y la aplicamos
        rb.velocity = modelTransform.TransformDirection(locVel);
        if(modelTransform.forward.y < maxSubida && modelTransform.forward.y>0)
        {
            rb.AddForce(-Physics.gravity * modelTransform.forward.y * GetComponent<Maneuverability>().AcelerationWithWeight, ForceMode.Acceleration);
        }


    }

    private void ApplyTurbo()
    {
        GetComponent<Rigidbody>().AddForce(modelTransform.forward * GetComponent<Turbo>().impulse * GetComponent<Maneuverability>().currentBoost, ForceMode.Acceleration);
    }

    public bool AnyMovementKeys
    {
        get { return (inputManager.Accelerate() != 0); }
    }

    public float VelocityFormula    //devuelve la velocidad máxima de la nave aplicando todos los modificadores
    {
        get { return Mathf.Clamp(GetComponent<Maneuverability>().MaxVelocity + HealthFormula + PositionFormula + RecoilFormula + BoostFormula, 0, Mathf.Infinity); }
    }

    public float BoostFormula
    {
        get { return ((turboConst * (inBoost == true ? 1 : 0) * GetComponent<Maneuverability>().currentBoost)); }
    }

    public float RecoilFormula
    {
        get { return ((recoilConst * (inRecoil == true ? 1 : 0) * GetComponent<Maneuverability>().currentRecoil)); }
    }

    public float PositionFormula
    {
        get { return (DistanciaPrimero * positionConst); }
    }

    public float HealthFormula
    {
        get { return (PorcentajeSalud * healthConst); }
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
