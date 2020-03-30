using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class LevelTools : MonoBehaviour
{
    [Header("TAG")]
    public string childTag = "Untagged";

    [Header("SNAP")]
    public LayerMask groundLayers;
    public float heightDiference = -0.1f;

    [Header("REPLACE")]
    public GameObject replaceObject;

    [Header("ROTATE")]
    public float minYRotation = 0f;
    public float maxYRotation = 360f;

    public float minZRotation = -5f;
    public float maxZRotation = 5;

    public float minXRotation = -5f;
    public float maxXRotation = 5;

    [Header("SCALE")]
    public float minScale = 0.65f;
    public float maxScale = 0.8f;
}
