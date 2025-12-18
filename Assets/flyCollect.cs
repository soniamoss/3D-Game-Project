using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyCollect : MonoBehaviour
{
    private SpiderDiner gameManager;
    public int value = 1;   // how many hearts this fly gives (if you want healing)
    float startY;

    void Start()
    {
        // Find the SpiderDiner manager in the scene (health + flies UI)
        gameManager = FindObjectOfType<SpiderDiner>();

        // Tell SpiderDiner that this fly exists
        if (gameManager != null)
        {
            gameManager.RegisterFly();
        }

        // Remember starting height for bobbing
        startY = transform.position.y;
    }

    void Update()
    {
        // Simple bobbing animation
        transform.position = new Vector3(
            transform.position.x,
            startY + Mathf.Sin(Time.time * 2f) * 0.25f,
            transform.position.z
        );
    }

    void OnTriggerEnter(Collider other)
    {
        // Only the spider (player) can collect this
        if (!other.CompareTag("Player")) return;

        if (gameManager != null)
        {
            // Count this fly as collected (updates UI + win check)
            gameManager.FlyCollected();

            // If flies should give health, keep this:
            gameManager.GainHealth(value);
            // If you don't want flies to give health, delete the line above.
        }

        // Remove fly from the scene
        Destroy(gameObject);
    }
}


