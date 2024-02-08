using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public float objectHealth = 120f;

    public void objectHitDamage(float amount)
    {
        objectHealth -= amount;

        if (objectHealth <= 0)
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
