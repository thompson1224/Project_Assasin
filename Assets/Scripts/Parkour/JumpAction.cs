using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour Menu/Create New Parkour Action")]
public class JumpAction : ScriptableObject
{
    [Header("Checking Obstacle Height")]
    [SerializeField] private string animationName;
    [SerializeField] private string barrierTag;
    [SerializeField] private float minmumHeight;
    [SerializeField] private float maximumHeight;
    
    [Header("Rotating Player towards Obstacle")]
    [SerializeField] bool lookAtObstacle;
    [SerializeField] private float parkourActionDelay;
    public Quaternion RequiredRotation { get; set; }

    [Header("Target Matching")] 
    [SerializeField] private bool allowTargetMatching = true;
    [SerializeField] private AvatarTarget compareBodyPart;
    [SerializeField] private float compareStartTime;
    [SerializeField] private float compareEndTime;
    public Vector3 ComparePostion { get; set; }
    [SerializeField] private Vector3 comparePositionWeight = new Vector3(0, 1, 0);
    
    public bool CheckAvailable(ObstacleInfo hitData, Transform player)
    {
        //Check Barrier Tag
        if (!string.IsNullOrEmpty(barrierTag) && hitData.hitInfo.transform.tag!= barrierTag)
        {
            return false;
        }
        
        // Check height of obstacle
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
    public float ParkourActionDelay => parkourActionDelay;
    public bool AllowTargetMathcing => allowTargetMatching;
    public AvatarTarget CompareBodyPart => compareBodyPart;
    public float CompareStartTime => compareStartTime;
    public float CompareEndTime => compareEndTime;
    public Vector3 ComparePositionWeight => comparePositionWeight;

}
