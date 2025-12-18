using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShoeSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject shoePrefab;

    [Header("Pooling")]
    public int poolSize = 20;

    [Header("Spawn Area")]
    public float spawnHeight = 20f;
    public float killHeight = -10f;
    public Vector2 xRange = new Vector2(-10f, 10f);

    [Header("Timing")]
    public float delayBetweenShoes = 0.5f;

    [Header("Falling Speed")]
    public float initialFallSpeed = 5f;
    public float speedIncreasePerSecond = 0.5f;
    public float maxFallSpeed = 25f;

    private List<GameObject> shoePool = new List<GameObject>();
    private int currentIndex = 0; // rotation index
    private GameObject activeShoe;
    private float elapsedTime;

    void Start()
    {
        // Create fixed pool of shoes
        for (int i = 0; i < poolSize; i++)
        {
            GameObject shoe = Instantiate(shoePrefab);
            shoe.SetActive(false);

            Rigidbody rb = shoe.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = shoe.AddComponent<Rigidbody>();
            }

            rb.useGravity = false; // we will control velocity manually
            shoePool.Add(shoe);
        }

        StartCoroutine(ShoeLoop());
    }

    IEnumerator ShoeLoop()
    {
        while (true)
        {
            // Wait until no active shoe
            while (activeShoe != null)
            {
                // Manually move the shoe downward
                Rigidbody rb = activeShoe.GetComponent<Rigidbody>();
                float fallSpeed = Mathf.Min(
                    initialFallSpeed + elapsedTime * speedIncreasePerSecond,
                    maxFallSpeed
                );
                rb.velocity = Vector3.down * fallSpeed;

                // Check for killHeight
                if (activeShoe.transform.position.y <= killHeight)
                {
                    RecycleShoe();
                }

                yield return null;
            }

            // Optional delay
            yield return new WaitForSeconds(delayBetweenShoes);

            SpawnShoe();
        }
    }

    void SpawnShoe()
    {
        activeShoe = shoePool[currentIndex];
        currentIndex = (currentIndex + 1) % poolSize;

        float randomX = Random.Range(xRange.x, xRange.y);

        activeShoe.transform.position = new Vector3(randomX, spawnHeight, 0f);
        activeShoe.transform.rotation = Quaternion.identity;
        activeShoe.SetActive(true);

        // Reset velocity
        Rigidbody rb = activeShoe.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void RecycleShoe()
    {
        activeShoe.SetActive(false);
        activeShoe = null;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }
}
