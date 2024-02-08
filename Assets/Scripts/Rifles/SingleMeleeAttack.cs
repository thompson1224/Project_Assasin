using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMeleeAttack : MonoBehaviour
{
    public int singleMeleeVal;
    public Animator anim;
    public PlayerScript playerScript;

    public Transform attackArea;
    public float giveDamage = 10f;
    public float attackRadius;
    public LayerMask knightLayer;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("SingleHandAttackActive", true);
        }
        SingleMeleeModes();
    }

    void SingleMeleeModes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            singleMeleeVal = Random.Range(1, 7);

            if (singleMeleeVal == 1)
            {
                Attack();
                StartCoroutine(SingleAttack1());
            }

            if (singleMeleeVal == 2)
            {
                Attack();
                StartCoroutine(SingleAttack2());
            }
            
            if (singleMeleeVal == 3)
            {
                Attack();
                StartCoroutine(SingleAttack3());
            }

            if (singleMeleeVal == 4)
            {

                Attack();
                StartCoroutine(SingleAttack4());
            }

            if (singleMeleeVal == 5)
            {

                Attack();
                StartCoroutine(SingleAttack5());
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

    IEnumerator SingleAttack1()
    {
        anim.SetBool("SingleAttack1", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack1", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator SingleAttack2()
    {
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        anim.SetBool("SingleAttack2", true);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack2", false); 
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }
    
    IEnumerator SingleAttack3()
    {
        anim.SetBool("SingleAttack3", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack3", false); 
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }
    
    IEnumerator SingleAttack4()
    {
        anim.SetBool("SingleAttack4", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack4", false); 
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }

    IEnumerator SingleAttack5()
    {
        anim.SetBool("SingleAttack4", true);
        playerScript.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0f);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("SingleAttack4", false);
        playerScript.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0f);
    }
}
