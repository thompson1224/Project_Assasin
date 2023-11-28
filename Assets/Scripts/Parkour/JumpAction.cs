using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour Menu/Create New Parkour Action")]
public class JumpAction : ScriptableObject
{
    [Header("Checking Obstacle Height")]
    [SerializeField] private string animationName;
    [SerializeField] private float minmumHeight;
    [SerializeField] private float maximumHeight;
    [SerializeField] bool lookAtObstacle;
    public Quaternion RequiredRotation { get; set; }

    [Header("Target Matching")] 
    [SerializeField] private bool allowTargetMatching = true;
    [SerializeField] private AvatarTarget compareBodyPart;
    [SerializeField] private float compareStartTime;
    [SerializeField] private float compareEndTime;
    public Vector3 ComparePostion { get; set; }
    
    
    public bool CheckAvailable(ObstacleInfo hitData, Transform player)
    {
        float checkHeight = hitData.heightInfo.point.y - player.position.y;

        if (checkHeight < minmumHeight || checkHeight > maximumHeight)
            return false;

        if (lookAtObstacle)
        {
            RequiredRotation = Quaternion.LookRotation(-hitData.hitInfo.normal);
        }

        if (allowTargetMatching)
        {
            ComparePostion = hitData.heightInfo.point;
        }

        return true;
    }

    public string AnimationName => animationName;
    public bool LookAtObstacle => lookAtObstacle;
    public bool AllowTargetMathcing => allowTargetMatching;
    public AvatarTarget CompareBodyPart => compareBodyPart;
    public float CompareStartTime => compareStartTime;
    public float CompareEndTime => compareEndTime;

}
