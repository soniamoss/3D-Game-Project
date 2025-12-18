using UnityEngine;
using System.Collections;

public class shoeDrop : MonoBehaviour
{
    public float fallDistance = 4.35f;
    public float fallSpeed = 1f;
    public float startDelay = 0f; // delay before this shoe starts falling

    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        startPos = transform.position;
        endPos = startPos + Vector3.down * fallDistance;

        StartCoroutine(FallRoutine());
    }

    IEnumerator FallRoutine()
    {
        // Wait before starting
        yield return new WaitForSeconds(startDelay);

        // Move down smoothly
        while (transform.position.y > endPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, fallSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = endPos; // ensure final position
    }
}
