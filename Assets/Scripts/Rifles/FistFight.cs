using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FistFight : MonoBehaviour
{
    public float timer = 0f;

    public int fistFightVal;
    public Animator anim;
    public PlayerScript playerScript;

    public Transform attackArea;
    public float giveDamage = 10f;
    public float attackRadius;
    public LayerMask knightLayer;

    [SerializeField] private Transform leftHandPunch;
    [SerializeField] private Transform rightHandPunch;
    [SerializeField] private Transform leftLegKick;
    
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            timer += Time.deltaTime;
        }
        else
        {
            // 전투시 속도 조절
            playerScript.movementSpeed = 3f; 
            anim.SetBool("FistFightActive", true);
            timer = 0f;
        }

        if (timer > 5f)
        {
            playerScript.movementSpeed = 5f;
            anim.SetBool("FistFightActive", false);
        }
        
        FistFightModes();
    }

    // Fist Fight Modes
    void FistFightModes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fistFightVal = Random.Range(1, 7);

            if (fistFightVal == 1)
            {
                //Attack
                attackArea = leftHandPunch;
                attackRadius = 0.5f;
                Attack();
                StartCoroutine(SingleFist());
            }

            if (fistFightVal == 2)
            {
                attackArea = rightHandPunch;
                attackRadius = 0.6f;
                Attack();
                StartCoroutine(DoubleFist());
            }
            
            if (fistFightVal == 3)
            {
                attackArea = leftHandPunch;
                attackArea = leftLegKick;
                attackRadius = 0.7f;
                Attack();
                StartCoroutine(FirstFistKick());
            }

            if (fistFightVal == 4)
            {
                attackArea = leftLegKick;
                attackRadius = 0.9f;
                Attack();
                StartCoroutine(KickCombo());
            }

            if (fistFightVal == 5)
            {
                attackArea = leftLegKick;
                attackRadius = 0.9f;
                Attack();
                StartCoroutine(LeftKick());
            }
        }
    }

    
    void Attack()
    {
        Collider[] hitKnight = Physics.OverlapSphere(attackArea.position, attackRadius, knightLayer);

        foreach (Collider knight in hitKnight)
        {
            KnightAI knightAI = knight.GetComponent<KnightAI>();

            if (knightAI != null)
            {
                knightAI.TakeDamage(giveDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackArea == null)
            return;
        Gizmos.DrawWireSphere(attackArea.position, attackRadius);
    }

    IEnumerator SingleFist()
    {
        anim.SetBool("SingleFist", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.7f);
        anim.SetBool("SingleFist", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator DoubleFist()
    {
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        anim.SetBool("DoubleFist", true);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("DoubleFist", false); 
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }
    
    IEnumerator FirstFistKick()
    {
        anim.SetBool("FirstFistKick", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("FirstFistKick", false); 
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }
    
    IEnumerator KickCombo()
    {
        anim.SetBool("KickCombo", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("KickCombo", false); 
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }
    
    IEnumerator LeftKick()
    {
        anim.SetBool("KickCombo", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("KickCombo", false); 
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }
}
