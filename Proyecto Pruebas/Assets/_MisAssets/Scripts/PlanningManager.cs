﻿using System.Collections;
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
    [Tooltip("Pon la velocidad a la que se mueve la nave lateralmente mientras planea, es un multiplicador")]
    public float lateralVelocity;


    private Rigidbody rb;   //rigidbody de la nave
    private InputManager inputManager;
    private NaveManager naveManager;
    private NaveController controller;
    private Maneuverability maneuverability;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        naveManager = GetComponent<NaveManager>();
        controller = GetComponent<NaveController>();
        maneuverability = GetComponent<Maneuverability>();
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
        float xRotation = controller.modelTransform.localEulerAngles.x;
        float zRotation = controller.modelTransform.localEulerAngles.z;

        if (naveManager.isPlanning)
        {
            //si el ángulo esta dentro del límite establecido y se tocan las teclas la nave se rota
            if ((xRotation <= maxXAngle + 0.1f && xRotation >= 0) || (xRotation <= 360 && xRotation >= 360 - 0.1f + minXAngle))
            {
                xRotation += inputManager.MainVertical() * Time.deltaTime * xSensivility * (verticalInverted ? -1 : 1);
                xRotation = Global.ClampAngle(xRotation, minXAngle, maxXAngle);
            }
            if ((zRotation <= maxZAngle + 0.1f && zRotation >= 0) || (zRotation <= 360 && zRotation >= 360 - 0.1f + minZAngle))
            {
                zRotation -= inputManager.MainHorizontal() * Time.deltaTime * zSensivility;
                zRotation = Global.ClampAngle(zRotation, minZAngle, maxZAngle);
            }
        }



        //si no se estan tocando las teclas la rotación vuelve a 0
        if (inputManager.MainHorizontal() == 0 || !naveManager.isPlanning)
        {
            zRotation = Mathf.LerpAngle(zRotation, 0, Time.deltaTime);
        }


        if (naveManager.isPlanning)
        {
            if (inputManager.MainVertical() == 0)
            {
                xRotation = Mathf.LerpAngle(xRotation, defaultXRotation, Time.deltaTime);
            }
        }
        else
        {
            xRotation = Mathf.LerpAngle(xRotation, 0, Time.deltaTime);
        }


        //modificamos la rotación del objeto
        controller.modelTransform.localRotation = Quaternion.Euler(xRotation, controller.modelTransform.localEulerAngles.y, zRotation);

        

        if (naveManager.isPlanning)
        {
            //añadimos la fuerza hacia delante
            rb.AddForce(controller.modelTransform.forward * naveManager.Acceleration * Mathf.Clamp01(inputManager.Accelerate()) * Time.deltaTime, ForceMode.VelocityChange);


            //movimiento lateral
            Vector3 lateralForce = controller.modelTransform.right;
            lateralForce = new Vector3(lateralForce.x, 0, lateralForce.z).normalized;
            //print(lateralForce);
            rb.AddForce(controller.modelTransform.right * lateralVelocity * inputManager.MainHorizontal());

            rb.AddForce(lateralForce * inputManager.MainHorizontal() * lateralVelocity * naveManager.Acceleration * Time.deltaTime, ForceMode.VelocityChange);
            //girar
            controller.modelTransform.localRotation = Quaternion.Euler(controller.modelTransform.localRotation.eulerAngles.x, controller.modelTransform.localRotation.eulerAngles.y + (inputManager.MainHorizontal() * naveManager.Maneuver * maneuverLimitator * Time.deltaTime), controller.modelTransform.localRotation.eulerAngles.z);

            RegulateVelocity();
        }
    }

    private void RegulateVelocity()
    {
        Vector3 locVel = controller.modelTransform.InverseTransformDirection(rb.velocity);

        //controlamos la velocidad no vertical para ponerle un tope
        /*Vector2 notVerticalVel = new Vector2(locVel.x, locVel.z);

        if (notVerticalVel.magnitude > GetComponent<NaveController>().VelocityFormula)
        {
            Vector2 correctedVel = notVerticalVel.normalized * GetComponent<NaveController>().VelocityFormula;

            //locVel = Vector3.Lerp( locVel,new Vector3(correctedVel.x, locVel.y, correctedVel.y),Time.deltaTime*1000);
            locVel = new Vector3(correctedVel.x, locVel.y, correctedVel.y);
        }*/

        if(locVel.z > controller.VelocityFormula)
        {
            locVel = new Vector3(locVel.x, locVel.y, controller.VelocityFormula );
        }
        if (locVel.x > controller.VelocityFormula*0.1f)
        {
            locVel = new Vector3(controller.VelocityFormula*0.1f, locVel.y, locVel.z);
        }
        //convertimos la velocidad local en la velocidad global y la aplicamos
        rb.velocity = controller.modelTransform.TransformDirection(locVel);
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

    

}
