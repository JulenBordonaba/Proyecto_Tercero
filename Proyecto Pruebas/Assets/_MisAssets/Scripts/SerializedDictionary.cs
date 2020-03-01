using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class SerializedDictionary :  ISerializationCallbackReceiver
{
    public List<TipoCombustible> _keys = new List<TipoCombustible>();
    public List<Material> _values = new List<Material>();

    //Unity doesn't know how to serialize a Dictionary
    public Dictionary<TipoCombustible, Material> fuelColors = new Dictionary<TipoCombustible, Material>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in fuelColors)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        fuelColors = new Dictionary<TipoCombustible, Material>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            fuelColors.Add(_keys[i], _values[i]);
    }

    void OnGUI()
    {
        foreach (var kvp in fuelColors)
            GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
    }
}
