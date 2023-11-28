using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")] 
    public float movementSpeed = 5f;
    public float rotSpeed = 600f;
    public MainCameraController MCC;
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
    [SerializeField] float fallingSpeed;
    [SerializeField] private Vector3 moveDir;
    
    void Update()
    {
        PlayerMovement();

        if (!playerControl) // cc가 해제되었을때는 미적용;
            return;
        
        if (onSurface)
        {
            fallingSpeed = -0.5f;
        }
        else
        {
            fallingSpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDir * movementSpeed;
        velocity.y = fallingSpeed;
        
        SurfaceCheck(); 
    }
    
    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        
        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;

        var movementDirection = MCC.flatRotation * movementInput;
        
        if (movementAmount > 0) // 움직일때
        {
            //transform.position += MovementDirection * movementSpeed * Time.deltaTime;
            CharacterControl.Move(movementDirection * movementSpeed * Time.deltaTime);
            requireRotation = Quaternion.LookRotation(movementDirection);
        }
        movementDirection = moveDir;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, requireRotation, rotSpeed * Time.deltaTime);

        animator.SetFloat("movementValue", movementAmount, 0.5f, Time.deltaTime); // 대기 - 걷기 전환간 부드럽게 하기 위함
    }

    void SurfaceCheck()
    {
        onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, surfaceLayer);
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
}
