using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveAnimationManager : MonoBehaviour
{
    [Tooltip("Pon el animator de la nave")]
    public Animator animator;
    [Header("Booleanas animator")]
    public bool move = false;
    public bool plane = false;

    private NaveController naveController;
    private NaveManager naveManager;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        naveController = GetComponent<NaveController>();
        inputManager = GetComponent<InputManager>();
        naveManager = GetComponent<NaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.inPause) return;
        animator.SetBool("move", move);
        animator.SetBool("plane", plane);
        animator.SetBool("inDrift", naveController.inDrift);
        Giro();
        Acceleration();
        //FuelState();
    }

    public void FuelState()
    {
        switch(naveManager.combustible.tipoCombustible)
        {
            case TipoCombustible.Escudo:
                animator.SetLayerWeight(animator.GetLayerIndex("Escudo"), 1);
                animator.SetLayerWeight(animator.GetLayerIndex("Salto"), 0);
                animator.SetLayerWeight(animator.GetLayerIndex("Reparar"), 0);
                animator.SetLayerWeight(animator.GetLayerIndex("Turbo"), 0);
                break;
            case TipoCombustible.Turbo:
                animator.SetLayerWeight(animator.GetLayerIndex("Escudo"), 0);
                animator.SetLayerWeight(animator.GetLayerIndex("Salto"), 0);
                animator.SetLayerWeight(animator.GetLayerIndex("Reparar"), 0);
                animator.SetLayerWeight(animator.GetLayerIndex("Turbo"), 1);
                break;
            case TipoCombustible.Reparar:
                animator.SetLayerWeight(animator.GetLayerIndex("Escudo"), 0);
                animator.SetLayerWeight(animator.GetLayerIndex("Salto"), 0);
                animator.SetLayerWeight(animator.GetLayerIndex("Reparar"), 1);
                animator.SetLayerWeight(animator.GetLayerIndex("Turbo"), 0);
                break;
            case TipoCombustible.Salto:
                animator.SetLayerWeight(animator.GetLayerIndex("Escudo"), 0);
                animator.SetLayerWeight(animator.GetLayerIndex("Salto"), 1);
                animator.SetLayerWeight(animator.GetLayerIndex("Reparar"), 0);
                animator.SetLayerWeight(animator.GetLayerIndex("Turbo"), 0);
                break;
            default:
                break;

        }
    }

    public void Acceleration()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Acelerar"), Mathf.Clamp01(inputManager.Accelerate()));
        animator.SetLayerWeight(animator.GetLayerIndex("Frenar"), Mathf.Clamp01(-inputManager.Accelerate()));
    }

    public void Giro()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Giro_Derecha"), Mathf.Clamp01(inputManager.MainHorizontal()));
        animator.SetLayerWeight(animator.GetLayerIndex("Giro_Izquierda"), Mathf.Clamp01(-inputManager.MainHorizontal()));
    }

}
