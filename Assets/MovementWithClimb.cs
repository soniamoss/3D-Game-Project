using UnityEngine;

public class MovementWithClimb : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 100f;
    public float climbSpeed = 100f;
    public float rotationSpeed = 10f;

    [Header("Climbing")]
    public float wallCheckDistance = 1f;
    public LayerMask climbableLayer;
    private Quaternion targetRotation;

    [Header("Jumping")]
    public float jumpForce = 7f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Wall Jump")]
    public float wallJumpUpForce = 7f;
    public float wallJumpPushForce = 5f;

    [Header("Camera")]
    public Transform mainCamera;
    private Rigidbody rb;
    private bool isClimbing = false;
    private Vector3 lastWallNormal;
    private Vector3 moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetMovementInput();

        // check if player is climbing
        if (isClimbing)
        {
            ClimbUpdate();
            WallJumpCheck();
        }
        else
        {
            CheckGround();
            GroundMoveUpdate();
            DetectWallForClimbing();
            JumpUpdate();
        }
    }



    // player movement, relative to which way the camera is facing
    void GetMovementInput()
    {
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) v = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) v = -1f;
        if (Input.GetKey(KeyCode.LeftArrow)) h = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) h = 1f;

        // get camera forward/right but flatten Y
        Vector3 camForward = mainCamera.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = mainCamera.right;
        camRight.y = 0f;
        camRight.Normalize();

        // gombine into movement direction
        moveDir = (camForward * v + camRight * h).normalized;
    }

    // running around on the ground
    void GroundMoveUpdate()
    {
        Vector3 horizontalVelocity = moveDir * moveSpeed;
        rb.velocity = new Vector3(
            horizontalVelocity.x,
            rb.velocity.y,
            horizontalVelocity.z
        );

        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
    }


    // transition from running on ground to climbing wall
    void DetectWallForClimbing()
    {
        if (moveDir == Vector3.zero) return;

        RaycastHit hit;

        // beging wall climbing mechanics
        if (Physics.Raycast(transform.position, moveDir, out hit, wallCheckDistance, climbableLayer))
        {
            StartClimbing(hit.normal);
        }
    }

    // beginning of wall climb
    void StartClimbing(Vector3 wallNormal)
    {
        isClimbing = true;
        lastWallNormal = wallNormal;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        // face the wall
        transform.rotation = Quaternion.LookRotation(-wallNormal);
    }

    // during climbing movement
    void ClimbUpdate()
    {
        // If wall is gone, stop climbing
        if (!Physics.Raycast(transform.position, -lastWallNormal, wallCheckDistance, climbableLayer))
        {
            StopClimbing();
            return;
        }

        float climbInput = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) climbInput = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) climbInput = -1f;

        Vector3 climbDir = transform.up * climbInput;

        rb.MovePosition(transform.position + climbDir * climbSpeed * Time.deltaTime);

        // Stay facing the wall
        Quaternion targetRot = Quaternion.LookRotation(-lastWallNormal);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 10f);
    }

    // end of climbing movement
    void StopClimbing()
    {
        isClimbing = false;
        rb.useGravity = true;
    }

    // check if player is in contact with the ground layer
    void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }

    // jumping mechanic
    void JumpUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // ability to jump off of a wall while climbing it
    void WallJumpCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // exit climb
            isClimbing = false;
            rb.useGravity = true;

            // clear old velocity
            rb.velocity = Vector3.zero;

            // jump away from wall + up
            Vector3 jumpDir = (-lastWallNormal * wallJumpPushForce) + (Vector3.up * wallJumpUpForce);

            rb.AddForce(jumpDir, ForceMode.Impulse);
        }
    }



}