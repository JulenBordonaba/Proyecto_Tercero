using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nave : Photon.PunBehaviour
{
    public GameObject cameraPrefab, cameraPivot; //prefab de la camara y pivote del cual se mueve alrededor
    public Transform cameraFront, cameraBack;   //tranforms a los que apunta la cámara dependiendo de si mira hacia delante o hacia atras

    public Camera myCamera;     //cámara del jugador
    public const float friction = 0.05f;    //valor de fricción artificial, ya que la nave esta flotando y no aplica ninguna fricción creamos una
    public const float backwardVelocity = 0.7f;     //multiplicador a la velocidad cuando la nave va marcha atrás

    public float levitationHeight = 1;      //variable que indica la altura a la que la nave estará flotando
    public float levitationForce;       //fuerza que se aplica a la nave para que levite en la altura designada
    public float startCorrectionHeight = 10;    //altura a la que la nave empezará a corregir la rotación para acabar cayendo paralela a la superficie
    public LineRenderer line;       //variable que guarda la línea que se usa para ver el recorrido del raycast de la nave al suelo
    public bool localUp = false;    //variable que altera el modo en el que la gravedad se aplica a la nave, para poder hacer looping o simplemente ir por el suelo
    public bool dmgInmune = false;      //Variable que hace a la nave inmune al daño
    public bool online = false;     //variable para pruebas de online

    public float constanteVelocidadPeso;    //variable para cálculos de velocidad
    public float constanteAceleracionPeso;      //variable para cálculos de aceleración

    [Header("Pruebas fuerzas")]
    public float fuerzaPorEncima;
    public float fuerzaPorDebajo;
    public float fuerzaPorMuyDebajo;

    [Header("Variables formula")]
    [Range(1, 10)]
    public float velocidadDerrape = 1;      //variable que modifica la cantidad de velocidad lateral que recive la nave en medio de un derrape

    [Range(0, 60)]
    public float maxInclination = 45f;      //variable que indica la cantidad máxima de grados que se puede inclinar la nave lateralmente

    [Range(0, 600)]
    public float alturaManejo = 30;         //variable que indica la altura del suelo a la que se puede manejar la nave

    [Range(0, 5)]
    public float impulsoExtraCaida = 2;     //variable que modifica el la velocidad con la que cae la nave, de lo contrario parece que cae muy lento


    //constantes de la fórmula de velocidad
    [Header("Constantes Formula Velocidad")]
    [Range(1, 10)]
    public float maxVel = 1;
    public float rebufoConst = 0;
    public float positionConst = 0; //este es el de la distancia con el primero
    public float healthConst = 0;
    public float boostConst = 0;

    //Estadísticas de la nave
    [Header("Stats")]
    [Tooltip("Vida representa la vida de la nave, si llega a 0 la pieza se destruye, Rango:0-500"), Range(0, 500)]
    public float vidaBase = 0;
    [Tooltip("Peso es el valor que representa el peso de la nave, Rango:0-500."), Range(0, 500)]
    public float pesoBase = 0;
    [Tooltip("Velocidad representa la velocidad máxima de la nave, Rango:0-500"), Range(0, 500)]
    public float velocidadBase = 0;
    [Tooltip("Acceleración representa la aceleración de la nave, Rango:0-500"), Range(0, 500)]
    public float aceleracionBase = 0;
    [Tooltip("Maniobrabilidad representa el manejo de la nave, lo rápido que gira y lo bien que derrapa, Rango:0-500"), Range(0, 500)]
    public float manejoBase = 0;
    [Tooltip("rebufo representa la velocidad que ganará la nave cuando este cogiendo rebufo, Rango:0-500"), Range(0, 500)]
    public float rebufoBase = 0;
    [Tooltip("Turbo representa la velocidad que gana la nave durante un turbo, Rango:0-500"), Range(0, 500)]
    public float turboBase = 0;
    [Tooltip("Dash Lateral representa la velocidad y distancia a la que la nave hace la carga lateral, Rango:0-500"), Range(0, 500)]
    public float dashLateralBase = 0;
    [Tooltip("Daño representa el daño que recive otra nave si colisiona con esta, RAngo:0-100"), Range(0, 100)]
    public float damageBase = 100;

    //lista con todas las piezas de la nave
    public List<Pieza> piezas = new List<Pieza>();
    /*
    //Sliders que muestran las estadísticas de la nave
    [Header("Sliders Velocidad")]
    public Slider sliderVelocidad;
    public Slider actualVelSlider;
    public Text maxVelText;
    public Text actualVelText;
    public Text maxVelWeightText;

    [Header("Sliders Aceleración")]
    public Slider sliderAceleration;
    public Slider actualAcelerationSlider;
    public Text maxAcelerationText;
    public Text actualAcelerationText;
    public Text maxAcelerationWeightText;


    [Header("Sliders Health")]
    public Slider sliderHealth;
    public Slider actualHealthSlider;
    public Text maxHealthText;
    public Text actualHealthText;
    //private float currentHealth;

    [Header("Sliders Manejo")]
    public Slider sliderManejo;
    public Slider actualManejo;
    public Text maxManejoText;
    public Text actualManejoText;

    [Header("Sliders Rebufo")]
    public Slider sliderRebufo;
    public Slider actualRebufoSlider;
    public Text maxRebufoText;
    public Text actualRebufoText;

    [Header("Sliders Turbo")]
    public Slider sliderTurbo;
    public Slider actualTurboSlider;
    public Text maxTurboText;
    public Text actualTurboText;
    

    [Header("Sliders Dash Lateral")]
    public Slider sliderDash;
    public Slider actualDashSlider;
    public Text maxDashText;
    public Text actualDashText;*/



    private Rigidbody rb;       //rigidbody de la nave
    private Transform piezasGameObject;     //transform del objeto que contiene el modelo con todas las piezas
    private bool inDerrape = false;     //variable que indica cuando la nave esta derrapando
    private float position = 0;     //variable que indica la posición de la nave en la carrera, sirve para hacer cálculos de velocidad
    private bool inRebufo = false;  //variable que indica cuando la nave esta cogiendo rebufo, sirve para hacer cálculos de velocidad
    private bool inBoost = false;   //variable que indica cuando la nave esta en un boost, sirve para hacer cálculos de velocidad
    private Pieza nucleo;   //variable que contiene el nucleo de la nave, si este se destruye la nave se destruye

    // Use this for initialization
    void Awake()
    {
        if (Global.myCam == null && online)
        {
            myCamera = Instantiate(cameraPrefab, cameraPivot.transform).GetComponent<Camera>();
            myCamera.GetComponent<CameraController>().localPos = new Vector3(0, 3.5f, -9.6f);
            myCamera.GetComponent<CameraController>().target = transform;
            myCamera.GetComponent<CameraController>().frontLookAt = cameraFront;
            myCamera.GetComponent<CameraController>().backLookAt = cameraBack;
            Camera.SetupCurrent(myCamera);
            Global.myCam = myCamera.gameObject;
        }
        //asignar valor a line
        if (!line)
        {
            line = GameObject.Find("Line").GetComponent<LineRenderer>();
        }
        //saveStartCorrectingHeight = startCorrectionHeight;

        piezas = new List<Pieza>(GetComponentsInChildren<Pieza>());
        piezasGameObject = piezas[0].transform.parent;
        rb = GetComponent<Rigidbody>();
        
        foreach (Pieza p in piezas)
        {
            //p.nave = this;
            if (p.nucleo)
            {
                nucleo = p;
            }
        }

        /*
        //Inicializar sliders
        rb.mass = pesoBase;
        sliderVelocidad.maxValue = Mathf.FloorToInt(MaxVelNoWeight);
        actualVelSlider.maxValue = Mathf.FloorToInt(MaxVelNoWeight);
        maxVelText.text = Mathf.FloorToInt(MaxVelNoWeight).ToString();

        sliderAceleration.maxValue = Mathf.FloorToInt(aceleracionBase);
        actualAcelerationSlider.maxValue = Mathf.FloorToInt(aceleracionBase);
        maxAcelerationText.text = Mathf.FloorToInt(aceleracionBase).ToString();

        sliderHealth.maxValue = Mathf.FloorToInt(1200);
        actualHealthSlider.maxValue = Mathf.FloorToInt(1200);
        actualHealthSlider.value = Mathf.FloorToInt(vidaBase);
        maxHealthText.text = Mathf.FloorToInt(vidaBase).ToString();

        sliderManejo.maxValue = Mathf.FloorToInt(300);
        actualManejo.maxValue = Mathf.FloorToInt(300);
        actualManejo.value = Mathf.FloorToInt(manejoBase);
        maxManejoText.text = Mathf.FloorToInt(manejoBase).ToString();

        sliderRebufo.maxValue = Mathf.FloorToInt(300);
        actualRebufoSlider.maxValue = Mathf.FloorToInt(300);
        actualRebufoSlider.value = Mathf.FloorToInt(rebufoBase);
        maxRebufoText.text = Mathf.FloorToInt(rebufoBase).ToString();

        sliderTurbo.maxValue = Mathf.FloorToInt(300);
        actualTurboSlider.maxValue = Mathf.FloorToInt(300);
        actualTurboSlider.value = Mathf.FloorToInt(turboBase);
        maxTurboText.text = Mathf.FloorToInt(turboBase).ToString();
        

        sliderDash.maxValue = Mathf.FloorToInt(300);
        actualDashSlider.maxValue = Mathf.FloorToInt(300);
        actualDashSlider.value = Mathf.FloorToInt(dashLateralBase);
        maxDashText.text = Mathf.FloorToInt(dashLateralBase).ToString();*/

    }

    // Update is called once per frame
    void Update()
    {
        

        if (!nucleo)
        {
            onNexusDestroyed();
        }
        if (!online)
        {
            Controller();
        }
        else if (photonView.isMine)
        {
            Controller();
        }
        /*
        //actualizar sliders
        sliderVelocidad.value = Mathf.FloorToInt(MaxVelocity);
        actualVelText.text = Mathf.FloorToInt(new Vector2(transform.InverseTransformDirection(rb.velocity).x, transform.InverseTransformDirection(rb.velocity).z).magnitude).ToString();
        maxVelWeightText.text = Mathf.FloorToInt(MaxVelocity).ToString();
        actualVelSlider.value = Mathf.FloorToInt(new Vector2(transform.InverseTransformDirection(rb.velocity).x, transform.InverseTransformDirection(rb.velocity).z).magnitude);

        sliderAceleration.value = Mathf.FloorToInt(AcelerationWithWeight);
        actualAcelerationSlider.value = Mathf.FloorToInt(AcelerationWithWeight * Mathf.Abs(Input.GetAxis("Nave Vertical")));
        maxAcelerationWeightText.text = Mathf.FloorToInt(AcelerationWithWeight).ToString();
        actualAcelerationText.text = Mathf.FloorToInt(AcelerationWithWeight * Mathf.Abs(Input.GetAxis("Nave Vertical"))).ToString();

        sliderHealth.value = Mathf.FloorToInt(vidaBase);
        actualHealthSlider.value = Mathf.FloorToInt(currentHealth);
        maxHealthText.text = Mathf.FloorToInt(vidaBase).ToString();
        actualHealthText.text = Mathf.FloorToInt(currentHealth).ToString();

        sliderManejo.value = Mathf.FloorToInt(manejoBase);
        actualManejo.value = Mathf.FloorToInt(manejoBase);
        maxManejoText.text = Mathf.FloorToInt(manejoBase).ToString();
        actualManejoText.text = Mathf.FloorToInt(manejoBase).ToString();

        sliderRebufo.value = Mathf.FloorToInt(rebufoBase);
        actualRebufoSlider.value = Mathf.FloorToInt(rebufoBase);
        maxRebufoText.text = Mathf.FloorToInt(rebufoBase).ToString();
        actualRebufoText.text = Mathf.FloorToInt(rebufoBase).ToString();

        sliderTurbo.value = Mathf.FloorToInt(turboBase);
        actualTurboSlider.value = Mathf.FloorToInt(turboBase);
        maxTurboText.text = Mathf.FloorToInt(turboBase).ToString();
        actualTurboText.text = Mathf.FloorToInt(turboBase).ToString();
        

        sliderDash.value = Mathf.FloorToInt(dashLateralBase);
        actualDashSlider.value = Mathf.FloorToInt(dashLateralBase);
        maxDashText.text = Mathf.FloorToInt(dashLateralBase).ToString();
        actualDashText.text = Mathf.FloorToInt(dashLateralBase).ToString();

        */




    }


    private void FixedUpdate()
    {

        Levitate();


    }


    private void Controller()
    {

        Camera.SetupCurrent(myCamera);

        //convertimos la velocidad de global a local
        Vector3 locVel = piezasGameObject.InverseTransformDirection(rb.velocity);

        //depende de la velocidad la camara esta mas o menos cerca del coche
        myCamera.gameObject.GetComponent<CameraController>().velocityOffset = new Vector3(0, 0, Mathf.Clamp(locVel.z / (Velocity() / 15), -4f, 5f));
        myCamera.fieldOfView = 60f + Mathf.Clamp(locVel.z / 15f, 0f, 80f);




        if (!inDerrape)
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

        ray.origin = piezasGameObject.position;//+ new Vector3(0, 0, Mathf.Clamp(locVel.z / (velocity / 10), -6f, 6f));
        if (localUp)
        {
            ray.direction = -piezasGameObject.up;
        }
        else
        {
            ray.direction = -Vector3.up;
        }

        //si el vehiculo esta cerca del suelo 
        if (Physics.Raycast(ray, out hit, alturaManejo, LayerMask.GetMask("Floor")))
        {

            //mover hacia adelante
            if (Input.GetAxis("Nave Vertical") > 0)
            {
                if (locVel.z < 0) // si estas moviendote hacia atras y quieres ir hacia adelante se ayuda a parar el vehiculo
                {
                    //rb.velocity = new Vector3(rb.velocity.x * (1 - friction*2), rb.velocity.y, rb.velocity.z * (1 - friction*2));
                    locVel = new Vector3(locVel.x, locVel.y, locVel.z * (1 - (friction)));
                }
                if (inDerrape)
                {
                    if (Input.GetAxis("Horizontal") > 0)
                    {
                        if (locVel.x > 0)
                        {
                            locVel.x = 0;
                        }
                        rb.AddForce(piezasGameObject.forward * 0.2f * Input.GetAxis("Nave Vertical") * AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                        rb.AddForce(-piezasGameObject.right * velocidadDerrape * Input.GetAxis("Nave Vertical") * AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                    }
                    else if (Input.GetAxis("Horizontal") < 0)
                    {
                        if (locVel.x < 0)
                        {
                            locVel.x = 0;
                        }
                        rb.AddForce(piezasGameObject.forward * 0.2f * Input.GetAxis("Nave Vertical") * AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                        rb.AddForce(piezasGameObject.right * velocidadDerrape * Input.GetAxis("Nave Vertical") * AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
                    }
                    else
                    {
                        //rb.AddForce(transform.forward * Input.GetAxis("Nave Vertical") * Mathf.Pow(aceleration, 2) * Time.deltaTime, ForceMode.Impulse);
                    }

                }
                else
                {
                    rb.AddForce((piezasGameObject.forward * Input.GetAxis("Nave Vertical") * AcelerationWithWeight * Time.deltaTime), ForceMode.VelocityChange); //fuerza para moverte hacia adelante
                }



            }
            else if (Input.GetAxis("Nave Vertical") < 0)  //mover hacia atras
            {
                if (locVel.z > 0)// si estas moviendote hacia adelante y quieres ir hacia atras se ayuda a parar el vehiculo
                {
                    locVel = new Vector3(locVel.x, locVel.y, locVel.z * (1 - (friction)));
                    //rb.velocity = new Vector3(rb.velocity.x * (1 - friction*2), rb.velocity.y, rb.velocity.z * (1 - friction*2));
                }
                rb.AddForce(piezasGameObject.forward * Input.GetAxis("Nave Vertical") * Mathf.Pow(aceleracionBase, 2) * backwardVelocity * Time.deltaTime, ForceMode.Impulse); // fuerza para moverte hacia atras


            }



            //rotación al girar

            if (Input.GetAxis("Nave Vertical") >= 0)
            {
                piezasGameObject.localRotation = Quaternion.Euler(piezasGameObject.localRotation.eulerAngles.x, piezasGameObject.localRotation.eulerAngles.y + (Input.GetAxis("Horizontal") * manejoBase * Time.deltaTime), piezasGameObject.localRotation.eulerAngles.z);
            }
            else if (Input.GetAxis("Nave Vertical") < 0)
            {
                piezasGameObject.localRotation = Quaternion.Euler(piezasGameObject.localRotation.eulerAngles.x, piezasGameObject.localRotation.eulerAngles.y - (Input.GetAxis("Horizontal") * manejoBase * Time.deltaTime), piezasGameObject.localRotation.eulerAngles.z);
            }


            //derrape
            if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.LeftShift))
            {
                if (locVel.z > 0)
                {
                    inDerrape = true;
                    myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = 0.3f;
                    //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (Input.GetAxis("Horizontal") * manejo * Time.deltaTime * 2), transform.rotation.eulerAngles.z);
                }
                else
                {
                    myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = 1f;
                    inDerrape = false;
                }
            }
            else
            {
                myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = 1f;
                inDerrape = false;
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
            inDerrape = false;



        }


        if (localUp)
        {
            if (!Physics.Raycast(ray, out hit, levitationHeight * 2, LayerMask.GetMask("Floor")))
            {
                //Vector3 rayDistance = ray.origin - hit.point; //guardamos la distancia del raycast
                rb.AddForce(-piezasGameObject.up * impulsoExtraCaida, ForceMode.VelocityChange);
                //if(rayDistance.magnitude/2<levitationHeight)
                //{
                //    startCorrectionHeight = saveStartCorrectingHeight;
                //}

            }
        }
        else
        {
            if (!Physics.Raycast(ray, out hit, levitationHeight * 2, LayerMask.GetMask("Floor")))
            {
                //Vector3 rayDistance = ray.origin - hit.point; //guardamos la distancia del raycast
                rb.AddForce(-Vector3.up * impulsoExtraCaida, ForceMode.VelocityChange);
                //if(rayDistance.magnitude/2<levitationHeight)
                //{
                //    startCorrectionHeight = saveStartCorrectingHeight;
                //}

            }
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
        if (notVerticalVel.magnitude > MaxVelocity)
        {
            Vector2 correctedVel = notVerticalVel.normalized * MaxVelocity;

            locVel = new Vector3(correctedVel.x, locVel.y, correctedVel.y);
        }

        //convertimos la velocidad local en la velocidad global y la aplicamos
        rb.velocity = piezasGameObject.TransformDirection(locVel);



    }



    private void Levitate()
    {
        Ray ray = new Ray();
        RaycastHit hit;

        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
        if (localUp)
        {
            ray.origin = piezasGameObject.position; //+ Vector3.ClampMagnitude((locVel.z * transform.forward), 5f);
            ray.direction = -transform.up;
        }
        else
        {
            ray.origin = piezasGameObject.position + Vector3.ClampMagnitude((locVel.z * transform.forward), 10f);
            ray.direction = -Vector3.up;
        }

        //lanzamos un raycast hacia el suelo
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Floor")))
        {
            Vector3 rayDistance = ray.origin - hit.point; //guardamos la distancia del raycast
            line.SetPosition(0, ray.origin); //ponemos un line renderer en el recorrido del raycast
            line.SetPosition(1, hit.point);

            float diference = rayDistance.magnitude - levitationHeight; //diferencia de altura entre la nave y la altura en la que queremos que esté


            if (!localUp)
            {

                rb.useGravity = true;
                //se le añade una fuerza para que flote a la altura que queremos
                if (rayDistance.magnitude < levitationHeight / 2)
                {
                    rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * fuerzaPorMuyDebajo), ForceMode.Acceleration);
                }
                else if (rayDistance.magnitude < levitationHeight)
                {
                    rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * fuerzaPorDebajo), ForceMode.Acceleration);
                }
                else
                {
                    rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * fuerzaPorEncima), ForceMode.Acceleration);
                }
            }
            else
            {
                rb.useGravity = false;
                //se le añade una fuerza para que flote a la altura que queremos
                if (rayDistance.magnitude < levitationHeight / 2)
                {
                    rb.AddForce((transform.up.normalized * levitationForce + transform.up.normalized * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * 20), ForceMode.Acceleration);
                }
                else if (rayDistance.magnitude < levitationHeight)
                {
                    rb.AddForce((transform.up.normalized * levitationForce + transform.up.normalized * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * 1), ForceMode.Acceleration);
                }
                else
                {
                    rb.AddForce((transform.up.normalized * levitationForce + transform.up.normalized * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * 1), ForceMode.Acceleration);
                }
            }


            Quaternion quaternionRot = transform.localRotation; //guardamos la rotación actual en quaternions

            transform.up = hit.normal; //hacemos que la nave este perpendicular a la normal del punto donde ha colisionado el raycast

            Quaternion quatNewRot = transform.localRotation;  //guardamos la rotación en quaternions despues de corregirla 
            Quaternion interpolation;
            //hacemos una interpolación entre la rotación inicial y la final en relación a la distancia al suelo
            if (!localUp)
            {
                interpolation = Quaternion.Lerp(quaternionRot, quatNewRot, (1 - ((rayDistance.magnitude - levitationHeight) / startCorrectionHeight)) * (1 / rayDistance.magnitude));
            }
            else
            {
                interpolation = Quaternion.Lerp(quaternionRot, quatNewRot, Time.deltaTime * 10);
            }
            //igualamos la rotación a el resultado de la interpolación
            transform.localRotation = interpolation;





            if (!localUp)
            {
                //si esta por debajo de la altura que queremos y rb.velocity.y<0  si esta por encima de la altura que queremos y rb.velocity.y>0 reducimos la velocidad del eje y
                if ((diference > 0 && rb.velocity.y > 0) || (diference < 0 && rb.velocity.y < 0))
                {
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.9f, rb.velocity.z);
                }
            }
            else
            {
                if ((diference > 0 && locVel.y > 0) || (diference < 0 && locVel.y < 0))
                {
                    locVel = transform.InverseTransformDirection(rb.velocity);
                    locVel = new Vector3(locVel.x, locVel.y * 0.9f, locVel.z);
                    rb.velocity = transform.TransformDirection(locVel);
                }
            }




        }

    }


    private void SurfaceChecker()
    {

        Ray ray = new Ray();
        RaycastHit hit;

        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
        if (localUp)
        {
            ray.origin = piezasGameObject.position; //+ Vector3.ClampMagnitude((locVel.z * transform.forward), 5f);
            ray.direction = -transform.up;
        }
        else
        {
            ray.origin = piezasGameObject.position + Vector3.ClampMagnitude((locVel.z * transform.forward), 5f);
            ray.direction = -Vector3.up;
        }
        //lanzamos un raycast hacia el suelo
        if (Physics.Raycast(ray, out hit, alturaManejo/2, LayerMask.GetMask("Floor")))
        {
            if (localUp) return;
            
            //comprobar la inclinación del terreno y si esta lo suficientemente inclinado cambiar a localUp

        }
        else
        {
            if (!localUp) return;
            localUp = false;
        }


    }

    private void onNexusDestroyed()
    {
        myCamera.gameObject.GetComponent<CameraController>().naveDestruida = true;
        Destroy(transform.parent);
    }

    private void OnTriggerEnter(Collider other)
    {
        //tenemos un trigger en la parte de abajo del vehiculo, en caso de que ese trigger llegue a tocar el suelo impulsa el vehiculo hacia arriba
        if (other.gameObject.tag == "Floor")
        {
            print("suelo");
            if (localUp)
            {
                rb.AddForce(transform.up * levitationForce * 100, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(Vector3.up * levitationForce * 100, ForceMode.Acceleration);
            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        print("relative velocity" + collision.relativeVelocity.magnitude);
        print("velocity" + rb.velocity.magnitude);
        List<Pieza> piezasColision = new List<Pieza>();
        foreach (Pieza p in piezas)
        {
            bool destroyed = false;
            foreach (ContactPoint cp in collision.contacts)
            {
                if (cp.thisCollider == p.gameObject.GetComponent<Collider>())
                {
                    if (cp.otherCollider.gameObject.GetComponent<Pieza>())
                    {
                        piezasColision.Add(cp.otherCollider.gameObject.GetComponent<Pieza>());

                    }
                    else if (cp.otherCollider.gameObject.GetComponent<EnviromentElement>())
                    {
                        //destroyed = p.Damage(CalculateObjectDamage(cp.otherCollider.gameObject.GetComponent<EnviromentElement>(), CollisionAngleValue(Vector3.Angle(rb.velocity, transform.position - collision.contacts[0].point))));
                        dmgInmune = true;
                        MakeVulnerable(0.5f);
                    }
                }
                if (destroyed) break;
            }

            if (piezasColision.Count > 0)
            {
                //float angle = Vector3.Angle(rb.velocity, piezasColision[0].nave.rb.velocity);
                float totalDmg = 0;
                foreach (Pieza pi in piezasColision)
                {
                    totalDmg += damageBase * pi.Importancia;
                }
                float dmg = totalDmg / piezasColision.Count;
                //destroyed = p.Damage(CalculateDamage(dmg, CollisionAngleValue(angle), piezasColision[0].nave));
            }

            if (destroyed) break;

        }
    }

    private float CalculateObjectDamage(EnviromentElement other, float angle)
    {
        float totalDamage = other.damage;

        print(totalDamage + " += " + other.damage + " * Mathf.Clamp((" + angle + " * (" + other.peso + " / " + 500f + ") + " + 1 + ") - ((" + rb.velocity.magnitude + " / " + 1000 + ") + " + angle + " * (" + pesoBase + " / " + 500 + ") + " + 1 + ")");
        totalDamage += other.damage * Mathf.Clamp((angle * (other.peso / 500) + 1) - ((rb.velocity.magnitude / 1000) + angle * (pesoBase / 500) + 1), 0, float.MaxValue);


        return totalDamage;
    }

    public void MakeVulnerable(float time)
    {
        StartCoroutine(MakeVulnerableCoroutine(time));
    }

    IEnumerator MakeVulnerableCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        dmgInmune = false;
    }

    

    public float CollisionAngleValue(float angle)
    {
        return 0.5f + ((Mathf.Repeat(angle, 360) > 180 ? (360 - Mathf.Repeat(angle, 360)) : Mathf.Repeat(angle, 360)) / 180);
    }

    public float CalculateDamage(float dmg, float angle, Nave other)
    {
        float totalDamage = dmg;
        print("a");
        totalDamage += dmg * (((other.rb.velocity.magnitude / 1000) + angle * (other.pesoBase / 500) + 1) - ((rb.velocity.magnitude / 1000) + angle * (pesoBase / 500) + 1));

        return totalDamage;
    }

    public bool AnyMovementKeys
    {
        get { return (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Joystick1Button2) || Input.GetAxis("Nave Vertical") != 0)/* || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)*/; }
    }

    public float VelocityFormula    //devuelve la velocidad máxima de la nave aplicando todos los modificadores
    {
        get { return MaxVelocity + (PorcentajeSalud * healthConst) + (DistanciaPrimero * positionConst) + ((rebufoConst * (inRebufo ? 1 : 0)) * Rebufo()) + ((boostConst * (inBoost ? 1 : 0)) * Turbo()); }
    }

    public float MaxVelocity    //devuelve la velocidad máxima de la nave sin aplicar modificadores por posición, rebufo, turbo y salud
    {
        get { return VelocityWithWeight * maxVel; }
    }

    public float MaxVelNoWeight //devuelve la velocidad de la nave sin ser afectada por el peso
    {
        get { return Velocity() * maxVel; }
    }

    public float DistanciaPrimero   //devuelve la distancia de la nave respecto al primero de la carrera
    {
        get { return 1; }
    }

    public float PorcentajeSalud    //devuelve el porcentaje de salud de la nave
    {
        get { return (nucleo.currentHealth / vidaBase) * 100; }
    }

    public float VelocityWithWeight //devuelve la velocidad base de la nave afectada por el peso
    {
        get { return Velocity() - (Peso() * constanteVelocidadPeso); }
    }

    public float AcelerationWithWeight  //devuelve la aceleración de la nave afectada por el peso
    {
        get { return Aceleracion() - (constanteAceleracionPeso * pesoBase); }
    }
    

    //Stats Actuales

    public float Velocity () 
    {
        float velocidad = velocidadBase;
        foreach(Pieza p in piezas)
        {
            if(!p.nucleo)
            {
                velocidad += velocidadBase * p.Importancia;
            }
        }
        return velocidad;
    }

    public float Aceleracion() 
    {
        float aceleracion = aceleracionBase;
        foreach (Pieza p in piezas)
        {
            if (!p.nucleo)
            {
                aceleracion += aceleracionBase * p.Importancia;
            }
        }
        return aceleracion;
    }

    public float Manejo() 
    {
        float manejo = manejoBase;
        foreach (Pieza p in piezas)
        {
            if (!p.nucleo)
            {
                manejo += manejoBase * p.Importancia;
            }
        }
        return manejo;
    }

    public float Peso() 
    {
        float peso = pesoBase;
        foreach (Pieza p in piezas)
        {
            if (!p.nucleo)
            {
                peso += pesoBase * p.Importancia;
            }
        }
        return peso;
    }

    public float DashLateral()
    {
        float dashLateral = dashLateralBase;
        foreach (Pieza p in piezas)
        {
            if (!p.nucleo)
            {
                dashLateral += dashLateralBase * p.Importancia;
            }
        }
        return dashLateral;
    }

    

public float Damage() 
    {
        float damage = damageBase;
        foreach (Pieza p in piezas)
        {
            if (!p.nucleo)
            {
                damage += damageBase * p.Importancia;
            }
        }
        return damage;
    }

    public float Turbo()
    {
        float turbo = turboBase;
        foreach (Pieza p in piezas)
        {
            if (!p.nucleo)
            {
                turbo += turboBase * p.Importancia;
            }
        }
        return turbo;
    }

    public float Rebufo()
    {
        float rebufo = rebufoBase;
        foreach (Pieza p in piezas)
        {
            if (!p.nucleo)
            {
                rebufo += rebufoBase * p.Importancia;
            }
        }
        return rebufo;
    }

}
