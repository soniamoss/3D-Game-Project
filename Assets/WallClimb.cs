using UnityEngine;

public class WallClimb : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 30f;
    public float turnSpeed = 15f;

    [Header("Wall Climbing")]
    public float wallDetectDistance = 1f;
    public float climbSpeed = 10f;
    public float stickDistance = 0.4f;
    public LayerMask climbableMask;

    private bool isClimbing = false;
    private Vector3 wallNormal;

    void Update()
    {
        if (isClimbing)
        {
            ClimbMovement();
            AlignToWall();
        }
        else
        {
            GroundMovement();
        }

        DetectWall();
    }

    // -----------------------------
    // GROUND MOVEMENT
    // -----------------------------
    void GroundMovement()
    {
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) v = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) v = -1f;
        if (Input.GetKey(KeyCode.LeftArrow)) h = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) h = 1f;

        Vector3 move = new Vector3(h, 0f, v).normalized;

        // Move in world space
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        // Rotate toward movement direction
        if (move != Vector3.zero)
        {
            Quaternion target = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, turnSpeed * Time.deltaTime);
        }
    }

    // -----------------------------
    // WALL CLIMB MOVEMENT
    // -----------------------------
    void ClimbMovement()
    {
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) v = 1f; 
        if (Input.GetKey(KeyCode.DownArrow)) v = -1f;
        if (Input.GetKey(KeyCode.LeftArrow)) h = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) h = 1f;

        // Move along wall using local axes
        Vector3 input = (transform.up * v + transform.right * h).normalized;

        transform.Translate(input * climbSpeed * Time.deltaTime, Space.World);
    }

    // -----------------------------
    // WALL ALIGNMENT
    // -----------------------------
    void AlignToWall()
    {
        // Rotate so the player faces the wall
        Quaternion targetRot = Quaternion.LookRotation(-wallNormal, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
    }

    // -----------------------------
    // WALL DETECTION
    // -----------------------------
    void DetectWall()
    {
        RaycastHit hit;

        // Look forward for wall
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectDistance, climbableMask))
        {
            // Make sure wall is not a floor or ceiling
            float verticality = Mathf.Abs(Vector3.Dot(hit.normal, Vector3.up));

            if (verticality < 0.7f) // surface must be mostly vertical
            {
                if (!isClimbing)
                    StartClimbing(hit.normal);

                wallNormal = hit.normal;

                // Snap to wall only when close enough
                if (hit.distance > stickDistance)
                    transform.position = hit.point + wallNormal * stickDistance;

                return;
            }
        }

        // If climbing but no wall found → stop
        if (isClimbing)
            StopClimbing();
    }

    // -----------------------------
    // START / STOP CLIMB
    // -----------------------------
    void StartClimbing(Vector3 normal)
    {
        isClimbing = true;
    }

    void StopClimbing()
    {
        isClimbing = false;
    }
}
