using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    public EnvironmentChecker environmentChecker;
    private bool playerInAction;
    public Animator animator;
    public PlayerScript playerScript;
    [SerializeField] private JumpAction jumpDownParkourAction;
    private float autoJumpHeightLimit = 2f;
    
    [Header("Jump Action Area")] 
    public List<JumpAction> newJumpActions;

    private void Update()
    {
        var hitData = environmentChecker.CheckObstacle();

        if (Input.GetButton("Jump") && !playerInAction)
        {
            if (hitData.hitFound)
            {
                foreach (var action in newJumpActions)
                {
                    if (action.CheckAvailable(hitData, transform))
                    {
                        StartCoroutine(PerformParkourAction(action));
                        break;
                    }
                }
            }
        }

        // if player is on a ledge and is not in action and there is no obstacle in front of him
        if (playerScript.playerOnLedge && !playerInAction && !hitData.hitFound)
        {
            bool canJump = true;
            
            if (playerScript.LedgeInfo.height > autoJumpHeightLimit && !Input.GetButton("Jump"))
                canJump = false;
                
            if (canJump && playerScript.LedgeInfo.angle <= 90)
            {
                playerScript.playerOnLedge = false;
                StartCoroutine(PerformParkourAction(jumpDownParkourAction));
            }
        }
    }

    IEnumerator PerformParkourAction(JumpAction action)
    {
        playerInAction = true;
        playerScript.SetControl(false);
        
        animator.CrossFade(action.AnimationName, 0.2f);
        yield return null;
        var animationState = animator.GetNextAnimatorStateInfo(0);
        if (!animationState.IsName(action.AnimationName))
            Debug.Log("Animation Name is Incorrect.");
        
        //yield return new WaitForSeconds(animationState.length);
        float timerCounter = 0f;

        while (timerCounter <= animationState.length)
        {
            timerCounter += Time.deltaTime;

            // 캐릭터가 장애물을 바라보게 변경
            if (action.LookAtObstacle)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.RequiredRotation, playerScript.rotSpeed * Time.deltaTime);
            }

            if (action.AllowTargetMathcing)
            {
                CompareTarget(action);
            }

            if (animator.IsInTransition(0) && timerCounter >= 0.5f)
            {
                break;
            }
            
            yield return null;
        }

        yield return new WaitForSeconds(action.ParkourActionDelay);
        
        playerScript.SetControl(true);
        playerInAction = false;
    }

    void CompareTarget(JumpAction action)
    {
        animator.MatchTarget(action.ComparePostion, transform.rotation, action.CompareBodyPart,
            new MatchTargetWeightMask(action.ComparePositionWeight, 0), action.CompareStartTime, action.CompareEndTime);
    }
}
