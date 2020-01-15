using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizableColor 
{
    public float r = 0;
    public float g = 0;
    public float b = 0;

    public Color ToColor()
    {
        return new Color(r, g, b);
    }

    public void ToSynchronizable(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
    }
}
