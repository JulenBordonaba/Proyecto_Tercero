using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Effects/Create Effect")]
public class EffectData : ScriptableObject
{
    public string id;
    public float duration;
    public float damageReduction;
    public float slow;
    public float velocity;
    public float maneuverability;
    public float acceleration;
    public float shotDamage;
    public DOT dot;

    #region PhotonSerialize
    public byte classId { get; set; }

    public static object Deserialize(byte[] data)
    {
        EffectData result = new EffectData();
        result.classId = data[0];
        return result;
    }

    public static byte[] Serialize(object customType)
    {
        EffectData c = (EffectData)customType;
        return new byte[] { c.classId };
    }
    #endregion
}

[System.Serializable]
public class DOT
{
    public float loopTime;
    public float damagePerTick;
    public Coroutine dotEffect;
}
