using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLife : MonoBehaviour
{
    //Use as a life tracker(max number of lives is 3)
    [Header("Lives")]
    public int maxLives = 3;
    private int currentLives;

    //Use this for importing sounds(for when spider is smushed by the shoes)
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip squishClip;

    //After we die, we should respawn from the starting position after a 1 second delay
    [Header("Respawn Settings")]
    public float respawnDelay = 1.0f;
    
    //Update the hearts with current health
    [Header("UI")]
    public SpiderDiner healthUI;

    private Vector3 startPos;
    private Quaternion startRot;
    private bool isRespawning = false;

    void Start()
    {
        currentLives = maxLives;

        //remember the starting position/rotation
        startPos = transform.position;
        startRot = transform.rotation;

        //auto-grab AudioSource if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    
    //Use this for when the shoe and spider collide
    void OnCollisionEnter(Collision collision)
    {
        if (isRespawning) return;

        //only react to objects tagged "Shoe"
        if (collision.gameObject.CompareTag("Shoe"))
        {
            GotHitByShoe();
        }
    }

    //If the spider is hit by a shoe the audio should play and spider should respawn
    void GotHitByShoe()
    {
        if (isRespawning) return;
        isRespawning = true;

        //play sound
        if (audioSource != null && squishClip != null)
        {
            audioSource.PlayOneShot(squishClip);
        }

        // ose a life
        currentLives--;
        Debug.Log("Spider hit by shoe! Lives left: " + currentLives);

        //update heart/health bar
        if (healthUI != null)
        {
            healthUI.LoseHealth();
        }

        //start the respawn
        StartCoroutine(RespawnAfterDelay());

        //Game over
        if (currentLives <= 0)
        {
            Debug.Log("Spider is out of lives! (TODO: game over screen)");
        }
    }

    IEnumerator RespawnAfterDelay()
    {
        //wait before respawning
        yield return new WaitForSeconds(respawnDelay);

        //respawn at start position
        transform.SetPositionAndRotation(startPos, startRot);

        //stop any leftover movement
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        isRespawning = false;
    }
}
