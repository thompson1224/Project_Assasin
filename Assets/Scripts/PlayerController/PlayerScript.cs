using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")] 
    public float movementSpeed = 5f;
    public float rotSpeed = 600f;
    public MainCameraController MCC;
    public EnvironmentChecker environmentChecker;
    private Quaternion requireRotation;
    private bool playerControl = true;

    [Header("Player Animator")] 
    public Animator animator;

    [Header("Player Collision / Gravity")] 
    public CharacterController CharacterControl;
    public float surfaceCheckRadius = 0.1f;
    public Vector3 surfaceCheckOffset;
    public LayerMask surfaceLayer;
    private bool onSurface;
    public bool playerOnLedge { get; set; }
    public LedgeInfo LedgeInfo { get; set; }
    [SerializeField] float fallingSpeed;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private Vector3 requiredMoveDir;
    private Vector3 velocity;
    
    void Update()
    {
        PlayerMovement();

        if (!playerControl) // cc가 해제되었을때는 미적용;
            return;
        
        velocity = Vector3.zero;
        if (onSurface)
        {
            fallingSpeed = -0.5f;
            velocity = moveDir * movementSpeed;
            playerOnLedge = environmentChecker.CheckLedge(moveDir, out LedgeInfo ledgeInfo);
            
            if (playerOnLedge)
            {
                LedgeInfo = ledgeInfo;
                playerLedgeMovement();
                Debug.Log("Player On Ledge");
            }
            
            animator.SetFloat("movementValue", velocity.magnitude / movementSpeed, 0.5f, Time.deltaTime); // 대기 - 걷기 전환간 부드럽게 하기 위함
        }
        else
        {
            fallingSpeed += Physics.gravity.y * Time.deltaTime;
            
            velocity = transform.forward * movementSpeed / 2;
        }

        velocity.y = fallingSpeed;
        
        SurfaceCheck(); 
        animator.SetBool("onSurface", onSurface);
    }
    
    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        
        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;

        requiredMoveDir = MCC.flatRotation * movementInput;
        
        CharacterControl.Move(velocity * Time.deltaTime);
        
        if (movementAmount > 0 && moveDir.magnitude > 0.2f) // 움직일때
        {
            requireRotation = Quaternion.LookRotation(moveDir);
        }

        moveDir = requiredMoveDir;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, requireRotation, rotSpeed * Time.deltaTime);
    }

    void SurfaceCheck()
    {
        onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, surfaceLayer);
    }

    void playerLedgeMovement()
    {
        float angle = Vector3.Angle(LedgeInfo.surfaceHit.normal, requiredMoveDir);

        if (angle < 90)
        {
            velocity = Vector3.zero;
            moveDir = Vector3.zero;
        }
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius);
    }

    public void SetControl(bool hasControl)
    {
        this.playerControl = hasControl;
        CharacterControl.enabled = hasControl;

        if (!hasControl)
        {
            animator.SetFloat("movementValue", 0f);
            requireRotation = transform.rotation;
        }
    }

    public bool HasPlayerControl
    {
        get => playerControl;
        set => playerControl = value;
    }
}
