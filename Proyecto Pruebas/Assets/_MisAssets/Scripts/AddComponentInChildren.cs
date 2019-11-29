using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddComponentInChildren : MonoBehaviour
{
    [Header("Make Obstacle parameters")]
    public float collisionDamage = 50f;
    public float minDamage;

    [ContextMenu("Make Obstacle")]
    public void MakeObstacle()
    {
        foreach(Collider c in GetComponentsInChildren<Collider>())
        {
            if(c.gameObject.tag=="Obstacle")
            {
                Stats s =c.gameObject.AddComponent<Stats>();
                s.collisionDamage = collisionDamage;
                DamageManager dm = c.gameObject.AddComponent<DamageManager>();
                dm.minDamage = minDamage;
            }
        }
    }
}
