using UnityEngine;
using System.Collections;

public class shoeDrop : MonoBehaviour
{
    //Set initial distance, speed, and starting delay for shoes
    public float fallDistance = 4.35f;
    public float fallSpeed = 1f;
    public float startDelay = 0f;

    private Vector3 startPos;
    private Vector3 endPos;

    //The code should cause the shoe to fall from the sky to the ground at a specified rate
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

    //This shows the actual fall action of the shoe, it waits, then drops smoothly, and ends at a final position.
    IEnumerator FallRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (transform.position.y > endPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, fallSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = endPos;
    }
}
