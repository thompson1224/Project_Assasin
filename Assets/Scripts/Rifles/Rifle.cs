using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")] 
    public Transform shootingArea;

    public float giveDamage = 10f;
    public float shootingRange = 100f;
    public Animator animator;
    public bool isMoving;
    public PlayerScript playerScript;

    [Header("Rifle Ammo And reloading")] 
    private int maxAmmo = 1;
    public int presentAmmo;
    public int mag;
    public float reloadTime;
    private bool setReload;
    public GameObject crossHair;
    
    void Start()
    {
        presentAmmo = maxAmmo;
    }
    
    private void Update()
    {
        if (animator.GetFloat("movementValue") > 0.001f)
        {
            isMoving = true;
        }
        else if (animator.GetFloat("movementValue") < 0.0999999f)
        {
            isMoving = false;
        }
        
        if (setReload)
            return;

        if (presentAmmo <= 0 && mag > 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        if (Input.GetMouseButtonDown(0) && isMoving == false)
        {
            animator.SetBool("RifleActive", true);
            animator.SetBool("Shooting", true);
            Shoot();
        }
        else if (!Input.GetMouseButtonDown(0))
        {
            animator.SetBool("Shooting", false);
        }

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("RifleAim", true);
            //crossHair.SetActive(true);
        }
        else if (!Input.GetMouseButton(1))
        {
            animator.SetBool("RifleAim", false);
            //crossHair.SetActive(false);
        }
    }
    
    void Shoot()
    {
        if (mag <= 0)
        {
            // show Out Ui
            return;
        }

        presentAmmo--;

        if (presentAmmo == 0)
        {
            mag--;
        }
        
        RaycastHit hitInfo;

        if (Physics.Raycast(shootingArea.position, shootingArea.forward, out hitInfo, shootingRange))
        {
            KnightAI knightAI = hitInfo.transform.GetComponent<KnightAI>();

            if (knightAI != null)
            {
                knightAI.TakeDamage(giveDamage);
            }
        }
    }

    IEnumerator Reload()
    {
        setReload = true;
        animator.SetFloat("movementValue", 0f);
        playerScript.movementSpeed = 0f;
        animator.SetBool("ReloadRifle", true);
        
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("ReloadRifle", false);
        presentAmmo = maxAmmo;
        setReload = false;
        animator.SetFloat("movementValue", 0f);
        playerScript.movementSpeed = 5f;
    }
}
