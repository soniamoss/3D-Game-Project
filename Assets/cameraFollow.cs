using UnityEngine;

public class CameraFollowBehindPlayer : MonoBehaviour
{
    public Transform target;                 // The player
    public Vector3 offset = new Vector3(0f, 3.5f, -8f);
    public float followSpeed = 10f;
    public float rotateSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // --- Position behind the player based on the player’s rotation ---
        Vector3 desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // --- Rotate camera to look where the player is looking ---
        Quaternion targetRotation = Quaternion.LookRotation(target.forward, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
