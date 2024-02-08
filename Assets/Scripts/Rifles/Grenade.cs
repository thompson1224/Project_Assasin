using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float grenadeTimer = 3f;
    public float radius = 10f;
    private float countDown;
    public float giveDamage = 120f;
    private bool hasExploded = false;

    public GameObject explosionEffect;
    
    private void Start()
    {
        countDown = grenadeTimer;
    }

    private void Update()
    {
        countDown -= Time.deltaTime;

        if (countDown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        // Show Effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Get nearby Objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            //Add Force
            
            //Damage
            Object obj = nearbyObject.GetComponent<Object>();

            if (obj != null)
            {
                obj.objectHitDamage(giveDamage);
            }
        }
        Destroy(gameObject);
    }
}
