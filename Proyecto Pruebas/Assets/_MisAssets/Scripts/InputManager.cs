using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectManager))]
public class InputManager : MonoBehaviour
{
    public int numPlayer = 1;

    private EffectManager effectManager;

    

    private void Start()
    {
        effectManager = GetComponent<EffectManager>();
    }

    private void Update()
    {
        if (effectManager)
        {
            print(effectManager.InvertControls + " --------------------------------------------- " + gameObject.name);
        }
        else
        {
            print("no hay effect manager " + gameObject.name);
        }
    }

    //eje horizontal del joystick principal
    public float MainHorizontal()
    {
        float r = 0;
        r += Input.GetAxis("PCMainHorizontal" + numPlayer.ToString());
        r += Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "MainHorizontal" + numPlayer.ToString());
        return r * InverseValue;
    }

    //eje vertical del joystick principal
    public float MainVertical()
    {
        float r = 0;
        r += Input.GetAxis("PCMainVertical" + numPlayer.ToString());
        r += Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "MainVertical" + numPlayer.ToString());
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
        r += Input.GetAxis("PCCameraHorizontal" + numPlayer.ToString()) * MouseSensitivityMultiplier;
        r += Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "CameraHorizontal" + numPlayer.ToString());
        return r*InverseValue;
    }

    //eje vertical del joystick de la cámara
    public float CameraVertical()
    {
        float r = 0;
        r += Input.GetAxis("PCCameraVertical" + numPlayer.ToString()) * MouseSensitivityMultiplier;
        r += Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "CameraVertical" + numPlayer.ToString());
        return r*InverseValue;
    }

    public float MouseSensitivityMultiplier
    {
        get { return 0.1f; }
    }

    //joystick que maneja la cámara
    public Vector3 CameraJoystick()
    {
        return new Vector3(CameraHorizontal(), CameraVertical());
    }

    //botón de derrape
    public bool Drift()
    {
        return Input.GetButton("PCDrift" + numPlayer.ToString()) || Input.GetButton(OptionsMenu.settings.controllerType.ToString() + "Drift" + numPlayer.ToString());
    }

    //botón que activa el dash derecho
    public bool RightDash()
    {
        if (effectManager == null) return false;
        return Input.GetButtonDown((effectManager.InvertControls ? "PCLeftDash":"PCRightDash") + numPlayer.ToString()) || Input.GetButtonDown(OptionsMenu.settings.controllerType.ToString() + (effectManager.InvertControls ? "LeftDash" : "RightDash") + numPlayer.ToString());
    }

    //botón que activa el dash izquierdo
    public bool LeftDash()
    {
        if (effectManager == null) return false;
        return Input.GetButtonDown((effectManager? "PCLeftDash" : (effectManager.InvertControls ? "PCRightDash":"PCLeftDash")) + numPlayer.ToString()) || Input.GetButtonDown(OptionsMenu.settings.controllerType.ToString() + (effectManager.InvertControls ? "RightDash" : "LeftDash") + numPlayer.ToString());
    }

    //botón para cambiar de cámara
    public bool ChangeCamera()
    {
        return Input.GetButtonDown("PCChangeCamera" + numPlayer.ToString()) || Input.GetButtonDown(OptionsMenu.settings.controllerType.ToString() + "ChangeCamera" + numPlayer.ToString());
    }

    //botón para usar el combustible
    public bool UseFuel()
    {
        return Input.GetButtonDown("PCUseFuel" + numPlayer.ToString()) || Input.GetButtonDown(OptionsMenu.settings.controllerType.ToString() + "UseFuel" + numPlayer.ToString());
    }

    //botón para activar/desactivar el mapa
    public bool Map()
    {
        return Input.GetButtonDown("PCMap" + numPlayer.ToString()) || Input.GetButtonDown(OptionsMenu.settings.controllerType.ToString() + "Map" + numPlayer.ToString());
    }

    public bool Pause()
    {
        return Input.GetButtonDown("PCPause" + numPlayer.ToString()) || Input.GetButtonDown(OptionsMenu.settings.controllerType.ToString() + "Pause" + numPlayer.ToString());
    }

    public bool PlayerAbility()
    {
        return Input.GetButtonDown("PCPlayerAbility" + numPlayer.ToString()) || Input.GetButtonDown(OptionsMenu.settings.controllerType.ToString() + "PlayerAbility" + numPlayer.ToString());
    }

    public bool ShipAbility()
    {
        return Input.GetButtonDown("PCShipAbility" + numPlayer.ToString()) || Input.GetButtonDown(OptionsMenu.settings.controllerType.ToString() + "ShipAbility" + numPlayer.ToString());
    }

    public bool Shot()
    {
        if (OptionsMenu.settings.controllerType == ControllerType.XBOXONE)
        {
            if (Input.GetButton("PCShot" + numPlayer.ToString()) || Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "Shot" + numPlayer.ToString()) > 0) return true;
        }
        else if (OptionsMenu.settings.controllerType == ControllerType.PS4)
        {
            return Input.GetButton("PCShot" + numPlayer.ToString()) || Input.GetButton(OptionsMenu.settings.controllerType.ToString() + "Shot" + numPlayer.ToString());
        }

        return Input.GetButton("PCShot" + numPlayer.ToString()) || Input.GetButton(OptionsMenu.settings.controllerType.ToString() + "Shot" + numPlayer.ToString());
    }

    public float Accelerate()
    {
        float r = 0;
        r += Input.GetAxis("PCAccelerate" + numPlayer.ToString());
        if (OptionsMenu.settings.controllerType == ControllerType.XBOXONE)
        {
            r += Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "Accelerate" + numPlayer.ToString());
            r += Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "Decelerate" + numPlayer.ToString());
        }
        else if (OptionsMenu.settings.controllerType == ControllerType.PS4)
        {
            
            r += Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "Accelerate" + numPlayer.ToString());
        }
        
        return r;
    }

    public float ChangeFuel()
    {
        float r = 0;
        r += Input.GetAxis("PCChangeFuel" + numPlayer.ToString());
        r += Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "ChangeFuel" + numPlayer.ToString());
        return r;
        
    }

    public bool ResetCamera()
    {
        return Input.GetButtonDown(OptionsMenu.settings.controllerType.ToString() + "ResetCamera" + numPlayer.ToString());
    }


    public bool UseJump()
    {
        return (Input.GetAxis("PCJumpBoost" + numPlayer.ToString()) > 0.8f) || (Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "JumpBoost" + numPlayer.ToString()) > 0.8f);
    }

    public bool UseShield()
    {
        return (Input.GetAxis("PCRepairShield" + numPlayer.ToString()) < -0.8f) || (Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "RepairShield" + numPlayer.ToString()) < -0.8f);
    }

    public bool UseBoost()
    {
        return (Input.GetAxis("PCJumpBoost" + numPlayer.ToString()) < -0.8f) || (Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "JumpBoost" + numPlayer.ToString()) < -0.8f);
    }

    public bool UseRepair()
    {
        return (Input.GetAxis("PCRepairShield" + numPlayer.ToString()) > 0.8f) || (Input.GetAxis(OptionsMenu.settings.controllerType.ToString() + "RepairShield" + numPlayer.ToString()) > 0.8f);
    }
    
    public float InverseValue
    {
        get { return effectManager==null? 1 :(effectManager.InvertControls ? -1f : 1f); }
    }
    



}
