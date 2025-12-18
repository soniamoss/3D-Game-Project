using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyCollect : MonoBehaviour
{
    private SpiderDiner gameManager;
    public int value = 1;
    float startY;

    void Start()
    {
        gameManager = FindObjectOfType<SpiderDiner>();
        if (gameManager != null)
        {
            gameManager.RegisterFly();
        }
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
        if (!other.CompareTag("Player")) return;

        if (gameManager != null)
        {
            gameManager.FlyCollected();
            gameManager.GainHealth(1); // if flies give life
        }

        Destroy(gameObject);
    }
}
