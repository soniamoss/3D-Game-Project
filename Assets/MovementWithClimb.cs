using UnityEngine;

public class PlayerMovementWithClimb : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 100f;
    public float climbSpeed = 100f;
    public float rotationSpeed = 10f;

    [Header("Climbing")]
    public float wallCheckDistance = 1f;
    public LayerMask climbableLayer;

    [Header("Camera")]
    public Transform mainCamera;

    private Rigidbody rb;
    private bool isClimbing = false;
    private Vector3 lastWallNormal;
    private Vector3 moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // face away from camera
        //transform.rotation = Quaternion.Euler(0, 180f, 0);
    }

    void Update()
    {
        GetMovementInput();

        if (isClimbing)
        {
            ClimbUpdate();
        }
        else
        {
            GroundMoveUpdate();
            DetectWallForClimbing();
        }
    }

    // ------------------------
    // CAMERA-RELATIVE INPUT
    // ------------------------
    void GetMovementInput()
    {
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) v = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) v = -1f;
        if (Input.GetKey(KeyCode.LeftArrow)) h = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) h = 1f;

        // Get camera forward/right but flatten Y
        Vector3 camForward = mainCamera.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = mainCamera.right;
        camRight.y = 0f;
        camRight.Normalize();

        // Combine into movement direction
        moveDir = (camForward * v + camRight * h).normalized;
    }

    // ------------------------
    // GROUND MOVEMENT
    // ------------------------
    void GroundMoveUpdate()
    {
        if (moveDir.magnitude > 0.1f)
        {
            rb.MovePosition(transform.position + moveDir * moveSpeed * Time.deltaTime);

            // Rotate player to movement direction
            Quaternion targetRot = Quaternion.LookRotation(moveDir);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    // ------------------------
    // WALL DETECTION USING MOVEMENT DIR
    // ------------------------
    void DetectWallForClimbing()
    {
        if (moveDir == Vector3.zero) return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, moveDir, out hit, wallCheckDistance, climbableLayer))
        {
            StartClimbing(hit.normal);
        }
    }

    // ------------------------
    // START CLIMBING
    // ------------------------
    void StartClimbing(Vector3 wallNormal)
    {
        isClimbing = true;
        lastWallNormal = wallNormal;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        // face the wall
        transform.rotation = Quaternion.LookRotation(-wallNormal);
    }

    // ------------------------
    // CLIMB UPDATE
    // ------------------------
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

    // ------------------------
    // STOP CLIMBING
    // ------------------------
    void StopClimbing()
    {
        isClimbing = false;
        rb.useGravity = true;
    }
}
