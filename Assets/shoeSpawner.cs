using UnityEngine;
using System.Collections;

public class ShoeDropSpawner : MonoBehaviour
{
    [Header("Shoe Prefabs")]
    public GameObject[] shoePrefabs;

    [Header("Spawn Timing")]
    public float startSpawnDelay = 2f;
    public float minimumSpawnDelay = 0.15f;
    public float accelerationMultiplier = 0.95f;

    [Header("Spawn Area")]
    public Vector3 spawnArea = new Vector3(4f, 0f, 4f);

    [Header("Fall Settings")]
    public float minFallSpeed = 0.8f;
    public float maxFallSpeed = 2.5f;

    private float currentSpawnDelay;

    void Start()
    {
        currentSpawnDelay = startSpawnDelay;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnShoe();

            yield return new WaitForSeconds(currentSpawnDelay);

            // Speed up over time
            currentSpawnDelay *= accelerationMultiplier;
            currentSpawnDelay = Mathf.Max(currentSpawnDelay, minimumSpawnDelay);
        }
    }

    void SpawnShoe()
    {
        if (shoePrefabs.Length == 0) return;

        GameObject prefab = shoePrefabs[Random.Range(0, shoePrefabs.Length)];

        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            spawnArea.y,
            Random.Range(-spawnArea.z, spawnArea.z)
        );

        GameObject shoe = Instantiate(
            prefab,
            transform.position + randomOffset,
            Random.rotation
        );

        // Randomize fall behavior
        ShoeFall fall = shoe.GetComponent<ShoeFall>();
        if (fall != null)
        {
            fall.fallSpeed = Random.Range(minFallSpeed, maxFallSpeed);
            fall.startDelay = Random.Range(0f, 0.3f);
        }
    }
}

