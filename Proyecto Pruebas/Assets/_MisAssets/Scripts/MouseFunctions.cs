using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFunctions : MonoBehaviour
{
    public void MouseVisible(bool isVisible)
    {
        Cursor.visible = isVisible;
    }

    public void MouseLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MouseUnlocked()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void MouseConfined()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}