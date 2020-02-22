using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizableGameObject
{

    public GameObject go;
    public List<GameObject> gos = new List<GameObject>();

    public byte classId { get; set; }

    public static object Deserialize(byte[] data)
    {
        SynchronizableGameObject result = new SynchronizableGameObject();
        result.classId = data[0];
        return result;
    }

    public static byte[] Serialize(object customType)
    {
        SynchronizableGameObject c = (SynchronizableGameObject)customType;
        return new byte[] { c.classId };
    }

    public SynchronizableGameObject()
    {
        go = null;
        gos = new List<GameObject>();
    }

    public SynchronizableGameObject(GameObject _go)
    {
        go = _go;
    }

    public SynchronizableGameObject(List<GameObject> _gos)
    {
        gos = _gos;
    }

}
