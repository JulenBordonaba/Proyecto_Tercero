using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningManager : MonoBehaviour
{
    [Tooltip("pon la máxima velocidad de caída, poner a -1 si no se quiere modificar")]
    public float maxFallVelocity;   //velocidad de caída máxima
    [Tooltip("pon el ángulo máximo que puede tomar el eje x")]
    public float maxXAngle;
    [Tooltip("pon el ángulo mínimo que puede tomar el eje x")]
    public float minXAngle;
    [Tooltip("pon el ángulo máximo que puede tomar el eje z")]
    public float maxZAngle;
    [Tooltip("pon el ángulo mínimo que puede tomar el eje z")]
    public float minZAngle;
    [Tooltip("pon la sensivilidad en el eje x (de delante a atras de la nave)")]
    public float xSensivility;
    [Tooltip("pon la sensivilidad en el eje z (de la izquierda a la derecha de la nave)")]
    public float zSensivility;
    [Tooltip("pon un valor de 0 a 1 para disminuir el manejo mientras planea, 0,7 lo disminuye hasta un 70% del total")]
    public float maneuverLimitator = 0.1f;
    [Tooltip("pon la rotación vertical por defecto")]
    public float defaultXRotation;
    [Tooltip("Variable que controla si el control vertical al planear esta invertido o no")]
    public bool verticalInverted = false;


    private Rigidbody rb;   //rigidbody de la nave



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        LimitateFallVelocity();
        Controller();

    }

    private void Controller()
    {
        //se actualiza el valor de las variables de rotación
        float xRotation = GetComponent<NaveController>().modelTransform.localEulerAngles.x;
        float zRotation = GetComponent<NaveController>().modelTransform.localEulerAngles.z;

        if (GetComponent<NaveManager>().isPlanning)
        {
            //si el ángulo esta dentro del límite establecido y se tocan las teclas la nave se rota
            if ((xRotation <= maxXAngle + 0.1f && xRotation >= 0) || (xRotation <= 360 && xRotation >= 360 - 0.1f + minXAngle))
            {
                xRotation += InputManager.MainVertical() * Time.deltaTime * xSensivility * (verticalInverted ? -1 : 1);
                xRotation = ClampAngle(xRotation, minXAngle, maxXAngle);
            }
            if ((zRotation <= maxZAngle + 0.1f && zRotation >= 0) || (zRotation <= 360 && zRotation >= 360 - 0.1f + minZAngle))
            {
                zRotation -= InputManager.MainHorizontal() * Time.deltaTime * zSensivility;
                zRotation = ClampAngle(zRotation, minZAngle, maxZAngle);
            }
        }



        //si no se estan tocando las teclas la rotación vuelve a 0
        if (InputManager.MainHorizontal() == 0 || !GetComponent<NaveManager>().isPlanning)
        {
            zRotation = Mathf.LerpAngle(zRotation, 0, Time.deltaTime);
        }


        if (GetComponent<NaveManager>().isPlanning)
        {
            if (InputManager.MainVertical() == 0)
            {
                xRotation = Mathf.LerpAngle(xRotation, defaultXRotation, Time.deltaTime);
            }
        }
        else
        {
            xRotation = Mathf.LerpAngle(xRotation, 0, Time.deltaTime);
        }


        //modificamos la rotación del objeto
        GetComponent<NaveController>().modelTransform.localRotation = Quaternion.Euler(xRotation, GetComponent<NaveController>().modelTransform.localEulerAngles.y, zRotation);

        if (GetComponent<NaveManager>().isPlanning)
        {
            //añadimos la fuerza hacia delante
            rb.AddForce(GetComponent<NaveController>().modelTransform.forward * GetComponent<Maneuverability>().AcelerationWithWeight * InputManager.Accelerate() * Time.deltaTime, ForceMode.VelocityChange);

        
            //movimiento lateral
            rb.AddForce(new Vector3(GetComponent<NaveController>().modelTransform.up.x, 0, GetComponent<NaveController>().modelTransform.up.z) * GetComponent<Maneuverability>().AcelerationWithWeight * Time.deltaTime, ForceMode.VelocityChange);
            //girar
            GetComponent<NaveController>().modelTransform.localRotation = Quaternion.Euler(GetComponent<NaveController>().modelTransform.localRotation.eulerAngles.x, GetComponent<NaveController>().modelTransform.localRotation.eulerAngles.y + (Input.GetAxis("Horizontal") * GetComponent<Maneuverability>().maneuver * maneuverLimitator * Time.deltaTime), GetComponent<NaveController>().modelTransform.localRotation.eulerAngles.z);

        }
    }
    //función que limita la velocidad vertical negativa
    private void LimitateFallVelocity()
    {
        if (maxFallVelocity < 0) return;
        if (rb.velocity.y < -maxFallVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, -maxFallVelocity, rb.velocity.z);
        }
    }

    public float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180)
        {
            inverse = !inverse;
            tmin -= 180;
        }
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        if (max > 180)
        {
            inverse = !inverse;
            tmax -= 180;
        }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
    }

}
