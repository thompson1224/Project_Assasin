using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    public EnvironmentChecker environmentChecker;
    private bool playerInAction;
    public Animator animator;
    public PlayerScript playerScript;

    [Header("Jump Action Area")] 
    public List<JumpAction> newJumpActions;

    private void Update()
    {
        if (Input.GetButton("Jump") && !playerInAction)
        {
            var hitData = environmentChecker.CheckObstacle();

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

            // 장애물을 바라보게 변경
            if (action.LookAtObstacle)
            {
                Quaternion.RotateTowards(transform.rotation, action.RequiredRotation, playerScript.rotSpeed * Time.deltaTime);
            }

            if (action.AllowTargetMathcing)
            {
                CompareTarget(action);
            }
            yield return null;
        }
        
        playerScript.SetControl(true);
        playerInAction = false;
    }

    void CompareTarget(JumpAction action)
    {
        animator.MatchTarget(action.ComparePostion, transform.rotation, action.CompareBodyPart,
            new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), action.CompareStartTime, action.CompareEndTime);
    }
}
