using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLife : MonoBehaviour
{
    [Header("Lives")]
    public int maxLives = 3;
    private int currentLives;

    [Header("Audio")]
    public AudioSource audioSource;   // drag your AudioSource here
    public AudioClip squishClip;      // drag your squeak clip here

    [Header("Respawn Settings")]
    public float respawnDelay = 1.0f; // seconds to wait before respawn

    [Header("UI")]
    public SpiderDiner healthUI;      // drag the object with SpiderDiner here

    private Vector3 startPos;
    private Quaternion startRot;
    private bool isRespawning = false;

    void Start()
    {
        currentLives = maxLives;

        // remember starting position/rotation
        startPos = transform.position;
        startRot = transform.rotation;

        // auto-grab AudioSource if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isRespawning) return;

        // only react to objects tagged "Shoe"
        if (collision.gameObject.CompareTag("Shoe"))
        {
            GotHitByShoe();
        }
    }

    void GotHitByShoe()
    {
        if (isRespawning) return;
        isRespawning = true;

        // play sound
        if (audioSource != null && squishClip != null)
        {
            audioSource.PlayOneShot(squishClip);
        }

        // lose a life internally
        currentLives--;
        Debug.Log("Spider hit by shoe! Lives left: " + currentLives);

        // update heart UI
        if (healthUI != null)
        {
            healthUI.LoseHealth();
        }

        // start delayed respawn
        StartCoroutine(RespawnAfterDelay());

        // if you want game-over logic later, you can add it here
        if (currentLives <= 0)
        {
            Debug.Log("Spider is out of lives! (TODO: game over screen)");
        }
    }

    IEnumerator RespawnAfterDelay()
    {
        // wait before respawning
        yield return new WaitForSeconds(respawnDelay);

        // respawn at start position
        transform.SetPositionAndRotation(startPos, startRot);

        // stop any leftover movement
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        isRespawning = false;
    }
}