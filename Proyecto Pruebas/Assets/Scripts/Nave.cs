using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nave : Photon.PunBehaviour
{
    public GameObject cameraPrefab,cameraPivot;
    public Transform cameraFront, cameraBack;

    public Camera myCamera;
    public const float friction = 0.05f;
    public const float backwardVelocity = 0.7f;

    public float levitationHeight = 1;
    public float levitationForce;
    public float startCorrectionHeight = 10;
    public LineRenderer line;
    public bool localUp = false;
    public bool dmgInmune = false;
    public bool online = false;


    [Range(1, 10)]
    public float velocidadDerrape = 1;

    [Range(0, 60)]
    public float maxInclination = 45f;

    [Range(0, 600)]
    public float alturaManejo = 30;

    [Range(0, 5)]
    public float impulsoExtraCaida = 2;

    [Range(1, 5)]
    public float rotationDamping = 2;

    [Header("Constantes Formula Velocidad")]
    [Range(1, 10)]
    public float maxVel = 1;
    public float rebufoConst = 0;
    public float positionConst = 0; //este es el de la distancia con el primero
    public float healthConst = 0;
    public float boostConst = 0;
    //la última constante es la velocidad maxima (C1)


    [Header("Stats")]
    [Tooltip("Vida representa la vida de la nave, si llega a 0 la pieza se destruye, Rango:0-500"), Range(0, 500)]
    public float vida = 0;
    [Tooltip("Peso es el valor que representa el peso de la nave, Rango:0-500."), Range(0, 500)]
    public float peso = 0;
    [Tooltip("Velocidad representa la velocidad máxima de la nave, Rango:0-500"), Range(0, 500)]
    public float velocidad = 0;
    [Tooltip("Acceleración representa la aceleración de la nave, Rango:0-500"), Range(0, 500)]
    public float aceleracion = 0;
    [Tooltip("Maniobrabilidad representa el manejo de la nave, lo rápido que gira, Rango:0-500"), Range(0, 500)]
    public float maniobrabilidad = 0;
    [Tooltip("rebufo representa la velocidad que ganará la nave cuando este cogiendo rebufo, Rango:0-500"), Range(0, 500)]
    public float rebufo = 0;
    [Tooltip("Turbo representa la velocidad que gana la nave durante un turbo, Rango:0-500"), Range(0, 500)]
    public float turbo = 0;
    [Tooltip("Derrape representa el valor del derrape de la nave, lo cerrado que es el derrape y la cantidad de energía que gana con él, Rango:0-500."), Range(0, 500)]
    public float derrape = 0;
    [Tooltip("Dash Lateral representa la velocidad y distancia a la que la nave hace la carga lateral, Rango:0-500"), Range(0, 500)]
    public float dashLateral = 0;


    public List<Pieza> piezas = new List<Pieza>();

    private float currentHealth;
    
    private Rigidbody rb;
    private Transform piezasGameObject;
    private bool inDerrape = false;
    private float position = 0;
    private bool inRebufo = false;
    private bool inBoost = false;
    private Pieza nucleo;

    // Use this for initialization
    void Start()
    {
        if(G.myCam == null && online)
        {
            myCamera = Instantiate(cameraPrefab, cameraPivot.transform).GetComponent<Camera>();
            myCamera.GetComponent<CameraController>().localPos = new Vector3(0, 3.5f, -9.6f);
            myCamera.GetComponent<CameraController>().target = transform;
            myCamera.GetComponent<CameraController>().frontLookAt = cameraFront;
            myCamera.GetComponent<CameraController>().backLookAt = cameraBack;
            Camera.SetupCurrent(myCamera);
            G.myCam = myCamera.gameObject;
        }
        
        if (!line)
        {
            line = GameObject.Find("Line").GetComponent<LineRenderer>();
        }
        //saveStartCorrectingHeight = startCorrectionHeight;
        currentHealth = vida;
        piezas = new List<Pieza>(GetComponentsInChildren<Pieza>());
        piezasGameObject = piezas[0].transform.parent;
        rb = GetComponent<Rigidbody>();
        CalculateStats();
        foreach (Pieza p in piezas)
        {
            p.nave = this;
            if(p.nucleo)
            {
                nucleo = p;
            }
        }

        rb.mass = peso;
    }

    // Update is called once per frame
    void Update()
    {
        if(!nucleo)
        {
            onNexusDestroyed();
        }
        if (photonView.isMine || !online)
        {
            Controller();
        }
        
    }


    private void FixedUpdate()
    {
        
            Levitate();
        
        
    }

    

    private void Controller()
    {

        Camera.SetupCurrent(myCamera);
        
        //convertimos la velocidad de global a local
        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);

        //depende de la velocidad la camara esta mas o menos cerca del coche
        myCamera.gameObject.GetComponent<CameraController>().velocityOffset = new Vector3(0, 0, Mathf.Clamp(locVel.z / (velocidad / 15), -4f, 5f));
        myCamera.fieldOfView = 60f + Mathf.Clamp(locVel.z / 15f, 0f, 80f);




        if (!inDerrape)
        {
            //disminuimos poco a poco la velocidad lateral para que no se vaya demasiado la nave
            locVel.x *= 0.95f;
        }













        Ray ray = new Ray();
        RaycastHit hit;

        ray.origin = piezasGameObject.position;//+ new Vector3(0, 0, Mathf.Clamp(locVel.z / (velocity / 10), -6f, 6f));
        ray.direction = -Vector3.up;

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
                        rb.AddForce(transform.forward * 0.2f * Input.GetAxis("Nave Vertical") * aceleracion * 100 * Time.deltaTime, ForceMode.Impulse);
                        rb.AddForce(-transform.right * velocidadDerrape * Input.GetAxis("Nave Vertical") * aceleracion * 100 * Time.deltaTime, ForceMode.Impulse);
                    }
                    else if (Input.GetAxis("Horizontal") < 0)
                    {
                        if (locVel.x < 0)
                        {
                            locVel.x = 0;
                        }
                        rb.AddForce(transform.forward * 0.2f * Input.GetAxis("Nave Vertical") * aceleracion * 100 * Time.deltaTime, ForceMode.Impulse);
                        rb.AddForce(transform.right * velocidadDerrape * Input.GetAxis("Nave Vertical") * aceleracion * 100 * Time.deltaTime, ForceMode.Impulse);
                    }
                    else
                    {
                        //rb.AddForce(transform.forward * Input.GetAxis("Nave Vertical") * Mathf.Pow(aceleration, 2) * Time.deltaTime, ForceMode.Impulse);
                    }

                }
                else
                {
                    rb.AddForce(transform.forward * Input.GetAxis("Nave Vertical") * aceleracion * 100  * Time.deltaTime, ForceMode.Impulse); //fuerza para moverte hacia adelante
                }



            }
            else if (Input.GetAxis("Nave Vertical") < 0)  //mover hacia atras
            {
                if (locVel.z > 0)// si estas moviendote hacia adelante y quieres ir hacia atras se ayuda a parar el vehiculo
                {
                    locVel = new Vector3(locVel.x, locVel.y, locVel.z * (1 - (friction)));
                    //rb.velocity = new Vector3(rb.velocity.x * (1 - friction*2), rb.velocity.y, rb.velocity.z * (1 - friction*2));
                }
                rb.AddForce(transform.forward * Input.GetAxis("Nave Vertical") * Mathf.Pow(aceleracion, 2) * backwardVelocity * Time.deltaTime, ForceMode.Impulse); // fuerza para moverte hacia atras


            }

            

            //rotación al girar

            if (Input.GetAxis("Nave Vertical") >= 0)
            {
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + (Input.GetAxis("Horizontal") * maniobrabilidad * Time.deltaTime), transform.localRotation.eulerAngles.z);
            }
            else if (Input.GetAxis("Nave Vertical") < 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - (Input.GetAxis("Horizontal") * maniobrabilidad * Time.deltaTime), transform.rotation.eulerAngles.z);
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
            }

        }
        else //si el vehiculo no esta cerca del suelo se añade una fuerza para que caiga más rapido (de lo contrario tarda mucho en caer)
        {
            myCamera.gameObject.GetComponent<CameraController>().cameraDampingMultiplayer = 1f;
            inDerrape = false;



        }


        if(localUp)
        {
            if (!Physics.Raycast(ray, out hit, levitationHeight * 2, LayerMask.GetMask("Floor")))
            {
                //Vector3 rayDistance = ray.origin - hit.point; //guardamos la distancia del raycast
                rb.AddForce(-transform.up * impulsoExtraCaida, ForceMode.VelocityChange);
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
        piezasGameObject.localEulerAngles = new Vector3(piezasGameObject.localEulerAngles.x, piezasGameObject.localEulerAngles.y, Mathf.LerpAngle(piezasGameObject.localEulerAngles.z, Mathf.Clamp(maxInclination * -Input.GetAxis("Horizontal") * (rb.velocity.magnitude / MaxVelocity) * (maniobrabilidad / 100), -maxInclination, maxInclination), Time.deltaTime * rotationDamping));

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
        rb.velocity = transform.TransformDirection(locVel);





    }

    private void Levitate()
    {
        Ray ray = new Ray();
        RaycastHit hit;

        Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
        if(localUp)
        {
            ray.origin = piezasGameObject.position; //+ Vector3.ClampMagnitude((locVel.z * transform.forward), 5f);
            ray.direction = -transform.up;
        }
        else
        {
            ray.origin = piezasGameObject.position + Vector3.ClampMagnitude((locVel.z * transform.forward), 5f);
            ray.direction = -Vector3.up;
        }

        if (localUp)
        {
            rb.AddForce(-Physics.gravity * (Vector3.up.y - transform.forward.y), ForceMode.Impulse);
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
                    rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * 20), ForceMode.Acceleration);
                }
                else if (rayDistance.magnitude < levitationHeight)
                {
                    rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * 1), ForceMode.Acceleration);
                }
                else
                {
                    rb.AddForce((Vector3.up * levitationForce + Vector3.up * levitationForce * (levitationHeight / rayDistance.magnitude) * (levitationHeight - rayDistance.magnitude) * 1), ForceMode.Acceleration);
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
                //rb.AddForce(falseGravityForce * (-transform.up)  ,ForceMode.Acceleration);
                //print("diference" + -diference);
            }
            





            Vector3 rot = transform.localEulerAngles; // guardamos la rotación actual en eulers
            Quaternion quaternionRot = transform.localRotation; //guardamos la rotación actual en quaternions

            Vector3 localNormal = transform.InverseTransformDirection(hit.normal);

            //transform.up = transform.TransformDirection(new Vector3(localNormal.x, transform.InverseTransformDirection(transform.up).y, localNormal.z));

            transform.up = hit.normal; //hacemos que la nave este perpendicular a la normal del punto donde ha colisionado el raycast
            
            //do
            //{
                Vector3 newRot = transform.localEulerAngles; //guardamos la nueva rotación en eulers

                float rotDiference = rot.y - newRot.y;  //guardamos la diferencia de rotación en el eje y


                transform.Rotate(0, rotDiference, 0, Space.Self); //se rota el equivalente a lo que se ha disviado al hacer la nave perpendicular a la normal del punto donde ha colisionado el raycast

                //hacemos que la rotación sea la x,z de despues de alinearlo con la normal y la y de antes de alinearlo
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rot.y, transform.localEulerAngles.z); //este paso no debería ser necesario, pero en las cuestas el rotate hacia mal la suma de angulos, y si no ponemos el rotate en este paso se aplica mal la rotación (odio las rotaciones en Unity)

            //} while (transform.localEulerAngles.y-rot.y>0.1f || transform.localEulerAngles.y-rot.y<-0.1f);










            Quaternion quatNewRot = transform.localRotation;  //guardamos la rotación en quaternions despues de corregirla 

            //hacemos una interpolación entre la rotación inicial y la final en relación a la distancia al suelo
            Quaternion interpolation = Quaternion.Lerp(quaternionRot, quatNewRot, (1 - ((rayDistance.magnitude - levitationHeight) / startCorrectionHeight)) * (1 / rayDistance.magnitude));
            //igualamos la rotación a el resultado de la interpolación
            transform.localRotation = interpolation;




            //if (diference > 0 && rb.velocity.y > 0)
            //{
            //    rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, float.MinValue, 1 / Mathf.Tan(rayDistance.magnitude)), rb.velocity.z);
            //}
            //if (diference < 0 && rb.velocity.y < 0)
            //{
            //    rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, 1 / Mathf.Tan(rayDistance.magnitude), float.MaxValue), rb.velocity.z);
            //}

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
            if(localUp)
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
                    if(cp.otherCollider.gameObject.GetComponent<Pieza>())
                    {
                        piezasColision.Add(cp.otherCollider.gameObject.GetComponent<Pieza>());
                        
                    }
                    else if(cp.otherCollider.gameObject.GetComponent<EnviromentElement>())
                    {
                        destroyed = p.Damage(cp.otherCollider.gameObject.GetComponent<EnviromentElement>().damage);
                    }
                }
                if (destroyed) break;
            }

            if(piezasColision.Count>0)
            {
                float angle = Vector3.Angle(rb.velocity, piezasColision[0].nave.rb.velocity);
                float totalDmg = 0;
                foreach (Pieza pi in piezasColision)
                {
                    totalDmg += pi.daño;
                }
                float dmg = totalDmg / piezasColision.Count;
                destroyed = p.Damage(CalculateDamage(dmg,CollisionAngleValue(angle),piezasColision[0].nave));
            }

            if (destroyed) break;
            
        }
    }

    public void CalculateStats()
    {
        peso += 0;
        vida +=0;
        velocidad += 0;
        aceleracion += 0;
        maniobrabilidad +=0;
        rebufo += 0;
        turbo += 0;
        derrape +=0;
        dashLateral += 0;
        foreach (Pieza p in piezas)
        {
            peso += p.peso;
            vida += p.vida;
            velocidad += p.velocidad;
            aceleracion += p.aceleracion;
            maniobrabilidad += p.maniobrabilidad;
            rebufo += p.rebufo;
            turbo += p.turbo;
            derrape += p.derrape;
            dashLateral += p.dashLateral;
        }
        rb.mass = peso;
    }

    public float CollisionAngleValue(float angle)
    {
        return 0.5f + ((Mathf.Repeat(angle,360)>180 ? (360- Mathf.Repeat(angle, 360)) : Mathf.Repeat(angle, 360)) /180 );
    }

    public float CalculateDamage(float dmg,float angle,Nave other)
    {
        float totalDamage = dmg;

        totalDamage += dmg * (((other.rb.velocity.magnitude / 1000) + angle * (other.peso/500) + 1) - ((rb.velocity.magnitude / 1000) + angle * (peso / 500) + 1));

        return totalDamage;
    }

    public bool AnyMovementKeys
    {
        get { return (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Joystick1Button2) || Input.GetAxis("Nave Vertical") != 0)/* || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)*/; }
    }

    public float VelocityFormula
    {
        get { return MaxVelocity + (PorcentajeSalud * healthConst) + (DistanciaPrimero * positionConst) + ((rebufoConst * (inRebufo ? 1 : 0))* rebufo) + ((boostConst * (inBoost ? 1 : 0))*turbo); }
    }

    public float MaxVelocity
    {
        get { return velocidad * maxVel; }
    }

    public float DistanciaPrimero
    {
        get { return 1; }
    }

    public float PorcentajeSalud
    {
        get { return (currentHealth / vida) * 100; }
    }
}
