using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Global 
{
    public static GameObject myCam=null;
    public static int numPlayers=1;
    public static int winner=0;
    public static List<string> winners;
    public static bool onePlayer = false;
    public static string myShipType = "Scavenger";

    public static float ClampAngle(float angle, float min, float max)
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


    public static byte[] SerializeToByteArray(this object obj)
    {
        if (obj == null)
        {
            return null;
        }
        var bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static T Deserialize<T>(this byte[] byteArray) where T : class
    {
        if (byteArray == null)
        {
            return null;
        }
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(byteArray, 0, byteArray.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = (T)binForm.Deserialize(memStream);
            return obj;
        }
    }

    public static T RandomEnumValue<T>()
    {
        Array values = Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(random);
    }

    /// <summary>
    /// Baraja la lista usando el metodo de Fisher-Yates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void barajar<T>(this IList<T> list)
    {

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    #region SaveLoadData

    public static void SaveData<T>(this T _data, string path)
    {
        string completePath = Path.Combine(Application.persistentDataPath, path);
        string jsonData = JsonUtility.ToJson(_data, true);


        File.WriteAllText(completePath, jsonData);
    }

    public static T LoadData<T>(this string path)
    {
        string completePath = Path.Combine(Application.persistentDataPath, path);

        if (!File.Exists(completePath))
        {
            return default(T);
        }

        string jsonData = File.ReadAllText(completePath);

        T data = JsonUtility.FromJson<T>(jsonData);

        return data;

    }

    public static void SaveDataPlayerPrefs<T>(this T _data, string dataKey)
    {
        string jsonData = JsonUtility.ToJson(_data, true);

        PlayerPrefs.SetString(dataKey, jsonData);

    }

    public static T LoadDataPlayerPrefs<T>(this string dataKey)
    {
        if (!PlayerPrefs.HasKey(dataKey)) return default(T);

        string _jsonData = PlayerPrefs.GetString(dataKey);
        T _data = JsonUtility.FromJson<T>(_jsonData);
        return _data;

    }
    #endregion


}
