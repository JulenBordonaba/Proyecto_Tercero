﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : Photon.PunBehaviour
{
    [Tooltip("Pon el radio del area en el que se detectan cosas")]
    public float areaRadius = 100;
    [Tooltip("Pon el objeto que aparecerá en el radar")]
    public GameObject radarObject;
    [Tooltip("Pon la cámara de la nave")]
    public Camera myCamera;
    [Tooltip("Pon el objeto del radar")]
    public GameObject radar;

    private SphereCollider areaCollider;
    public List<GameObject> objectsInArea = new List<GameObject>();
    private Dictionary<GameObject, GameObject> objectsInAreaIcons = new Dictionary<GameObject, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        areaCollider = GetComponent<SphereCollider>();
        areaCollider.radius = areaRadius;
        CheckpointManager.OnCheckpointUnlocked.AddListener(ChangeCheckpointColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine) return;
        ShowObjectsInArea();
    }

    private void ShowObjectsInArea()
    {
        for(int i=0; i< objectsInArea.Count;i++)
        {
            ShowObjectIcon(objectsInArea[i]);
        }
    }

    private void ShowObjectIcon(GameObject target)
    {
        if (target == null)
        {
            objectsInArea.Remove(target);
            StartCoroutine(DestroyOnEndOfFrame(target));
            return;
        }
        
        Vector3 objectScreenPosition = myCamera.WorldToScreenPoint(target.transform.position);
        objectScreenPosition = new Vector3(objectScreenPosition.x - (Screen.width * 0.5f), RadarY, 0);
        if(objectScreenPosition.x<RadarLeft || objectScreenPosition.x>RadarRight || myCamera.WorldToScreenPoint(target.transform.position).z<0)
        {
            objectsInAreaIcons[target].SetActive(false);
        }
        else
        {
            objectsInAreaIcons[target].SetActive(true);
        }

        objectsInAreaIcons[target].GetComponent<RectTransform>().localPosition = objectScreenPosition;
    }

    public void ChangeCheckpointColor()
    {
        foreach(GameObject go in objectsInArea)
        {
            if(go)
            {
                if (go.GetComponentInParent<Checkpoint>())
                {
                    StartCoroutine(ResetCheckpointColor(go));
                }
            }
            
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<RadarTarget>())
        {
            if(!other.gameObject.GetComponent<RadarTarget>().radars.Contains(this))
            {
                other.gameObject.GetComponent<RadarTarget>().radars.Add(this);
            }
            objectsInArea.Add(other.gameObject);
            GameObject nuevo = Instantiate(other.GetComponent<RadarTarget>().radarImage,radar.transform.parent);
            nuevo.transform.SetSiblingIndex(nuevo.transform.parent.childCount - 2);
            if(!objectsInAreaIcons.ContainsKey(other.gameObject))
            {
                objectsInAreaIcons.Add(other.gameObject, nuevo);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<RadarTarget>())
        {
            RemoveRadarTarget(other.gameObject.GetComponent<RadarTarget>());
        }
    }

    public void RemoveRadarTarget(RadarTarget rt)
    {
        if(rt.gameObject && rt.gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyOnEndOfFrame(rt.gameObject));
            objectsInArea.Remove(rt.gameObject);
            StartCoroutine(RemoveRadarOnEndOfFrame(rt));
            print("debería quitarlo");
        }
    }


    IEnumerator RemoveRadarOnEndOfFrame(RadarTarget rt)
    {
        yield return new WaitForEndOfFrame();
        rt.radars.Remove(this);
    }

    public IEnumerator ResetCheckpointColor(GameObject key)
    {
        GameObject objToDestroy = objectsInAreaIcons[key];
        yield return new WaitForEndOfFrame();

        objectsInAreaIcons.Remove(key);
        Destroy(objToDestroy);
        GameObject nuevo = Instantiate(key.GetComponent<RadarTarget>().radarImage, radar.transform.parent);
        nuevo.transform.SetSiblingIndex(nuevo.transform.parent.childCount - 2);
        objectsInAreaIcons.Add(key.gameObject, nuevo);
    }

    private IEnumerator DestroyOnEndOfFrame(GameObject key)
    {
        GameObject objToDestroy = objectsInAreaIcons[key];
        yield return new WaitForEndOfFrame();
        objectsInAreaIcons.Remove(key);
        Destroy(objToDestroy);
    }

    public float RadarY
    {
        get { return radar.GetComponent<RectTransform>().localPosition.y; }
    }

    public float RadarLeft
    {
        get { return radar.transform.GetChild(0).GetComponent<RectTransform>().rect.xMin; }
    }

    public float RadarRight
    {
        get { return radar.transform.GetChild(0).GetComponent<RectTransform>().rect.xMax; }
    }

}
