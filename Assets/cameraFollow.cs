using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    //Set the target as our spider
    public Transform target;

    //Set offset and speeed values for the camera location and movement
    public Vector3 offset = new Vector3(0f, 3.5f, -8f);
    public float followSpeed = 10f;
    public float rotateSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        //positioning of the camera
        Vector3 desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        //rotate the camera depending on the rotations of the spider
        Quaternion targetRotation = Quaternion.LookRotation(target.forward, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
