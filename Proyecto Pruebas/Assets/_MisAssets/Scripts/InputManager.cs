using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public int numPlayer = 1;

    private EffectManager effectManager;

    private void Start()
    {
        effectManager = GetComponent<EffectManager>();
    }

    //eje horizontal del joystick principal
    public float MainHorizontal()
    {
        float r = 0;
        r += Input.GetAxis("PCMainHorizontal" + numPlayer.ToString());
        r += Input.GetAxis("PS4MainHorizontal" + numPlayer.ToString());
        return r * InverseValue;
    }

    //eje vertical del joystick principal
    public float MainVertical()
    {
        float r = 0;
        r += Input.GetAxis("PCMainVertical" + numPlayer.ToString());
        r += Input.GetAxis("PS4MainVertical" + numPlayer.ToString());
        return r*InverseValue;
    }

    //joystick principal, usa x e y 
    public Vector3 MainJoystick()
    {
        return new Vector3(MainHorizontal(), MainVertical());
    }

    //eje horizontal del joystick de la cámara
    public float CameraHorizontal()
    {
        float r = 0;
        r += Input.GetAxis("PCCameraHorizontal" + numPlayer.ToString());
        r += Input.GetAxis("PS4CameraHorizontal" + numPlayer.ToString());
        return r*InverseValue;
    }

    //eje vertical del joystick de la cámara
    public float CameraVertical()
    {
        float r = 0;
        r += Input.GetAxis("PCCameraVertical" + numPlayer.ToString());
        r += Input.GetAxis("PS4CameraVertical" + numPlayer.ToString());
        return r*InverseValue;
    }

    //joystick que maneja la cámara
    public Vector3 CameraJoystick()
    {
        return new Vector3(CameraHorizontal(), CameraVertical());
    }

    //botón de derrape
    public bool Drift()
    {
        return Input.GetButton("PCDrift" + numPlayer.ToString()) || Input.GetButton("PS4Drift" + numPlayer.ToString());
    }

    //botón que activa el dash derecho
    public bool RightDash()
    {
        return Input.GetButtonDown((effectManager.InvertControls ? "PCLeftDash":"PCRightDash") + numPlayer.ToString()) || Input.GetButtonDown((effectManager.InvertControls ? "PS4LeftDash" : "PS4RightDash") + numPlayer.ToString());
    }

    //botón que activa el dash izquierdo
    public bool LeftDash()
    {
        return Input.GetButtonDown((effectManager.InvertControls ? "PCRightDash":"PCLeftDash") + numPlayer.ToString()) || Input.GetButtonDown((effectManager.InvertControls ? "PS4RightDash" : "PS4LeftDash") + numPlayer.ToString());
    }

    //botón para cambiar de cámara
    public bool ChangeCamera()
    {
        return Input.GetButtonDown("PCChangeCamera" + numPlayer.ToString()) || Input.GetButtonDown("PS4ChangeCamera" + numPlayer.ToString());
    }

    //botón para usar el combustible
    public bool UseFuel()
    {
        return Input.GetButtonDown("PCUseFuel" + numPlayer.ToString()) || Input.GetButtonDown("PS4UseFuel" + numPlayer.ToString());
    }

    //botón para activar/desactivar el mapa
    public bool Map()
    {
        return Input.GetButtonDown("PCMap" + numPlayer.ToString()) || Input.GetButtonDown("PS4Map" + numPlayer.ToString());
    }

    public bool Pause()
    {
        return Input.GetButtonDown("PCPause" + numPlayer.ToString()) || Input.GetButtonDown("PS4Pause" + numPlayer.ToString());
    }

    public bool PlayerAbility()
    {
        return Input.GetButtonDown("PCPlayerAbility" + numPlayer.ToString()) || Input.GetButtonDown("PS4PlayerAbility" + numPlayer.ToString());
    }

    public bool ShipAbility()
    {
        return Input.GetButtonDown("PCShipAbility" + numPlayer.ToString()) || Input.GetButtonDown("PS4ShipAbility" + numPlayer.ToString());
    }

    public bool Shot()
    {
        return Input.GetButton("PCShot" + numPlayer.ToString()) || Input.GetButton("PS4Shot" + numPlayer.ToString());
    }

    public float Accelerate()
    {
        float r = 0;
        r += Input.GetAxis("PCAccelerate" + numPlayer.ToString());
        r += Input.GetAxis("PS4Accelerate" + numPlayer.ToString());
        return r;
    }

    public float ChangeFuel()
    {
        float r = 0;
        r += Input.GetAxis("PCChangeFuel" + numPlayer.ToString());
        r += Input.GetAxis("PS4ChangeFuel" + numPlayer.ToString());
        return r;
        
    }

    public bool ResetCamera()
    {
        return Input.GetButtonDown("PS4ResetCamera" + numPlayer.ToString());
    }


    public bool UseJump()
    {
        return (Input.GetAxis("PCJumpBoost" + numPlayer.ToString()) > 0.8f) || (Input.GetAxis("PS4JumpBoost" + numPlayer.ToString()) > 0.8f);
    }

    public bool UseShield()
    {
        return (Input.GetAxis("PCRepairShield" + numPlayer.ToString()) < -0.8f) || (Input.GetAxis("PS4RepairShield" + numPlayer.ToString()) < -0.8f);
    }

    public bool UseBoost()
    {
        return (Input.GetAxis("PCJumpBoost" + numPlayer.ToString()) < -0.8f) || (Input.GetAxis("PS4JumpBoost" + numPlayer.ToString()) < -0.8f);
    }

    public bool UseRepair()
    {
        return (Input.GetAxis("PCRepairShield" + numPlayer.ToString()) > 0.8f) || (Input.GetAxis("PS4RepairShield" + numPlayer.ToString()) > 0.8f);
    }
    
    public float InverseValue
    {
        get { return effectManager.InvertControls ? -1f : 1f; }
    }
    



}
