using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyCollect : MonoBehaviour
{
    public int value = 1;
    float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            startY + Mathf.Sin(Time.time * 2f) * 0.25f,
            transform.position.z
        );
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player touched it
        if (other.CompareTag("Player"))
        {
            // Notify player or game manager
            SpiderDiner player = other.GetComponent<SpiderDiner>();
            if (player != null)
            {
                player.GainHealth(1);
            }

            // Destroy collectible
            Destroy(gameObject);
        }
    }
}
