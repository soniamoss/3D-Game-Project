using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDiner : MonoBehaviour
{

    [Header("Health Settings")]
    public GameObject healthPrefab;
    public int maxNumLives = 3;

    [Header("Layout")]
    public float spacing = 50f;

    private List<GameObject> healthList = new List<GameObject>();

    void Start()
    {
        CreateHealthBar();
    }

    void CreateHealthBar()
    {
        for (int i = 0; i < maxNumLives; i++)
        {
            GameObject heart = Instantiate(healthPrefab, transform);

            RectTransform rt = heart.GetComponent<RectTransform>();

            // Anchor to top-right
            rt.anchorMin = new Vector2(1, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(1, 1);

            // Position from top-right corner
            rt.anchoredPosition = new Vector2(i * spacing, 0);

            healthList.Add(heart);
        }
    }

    // Call this when player takes damage
    public void LoseHealth()
    {
        if (healthList.Count == 0) return;

        GameObject heart = healthList[healthList.Count - 1];
        healthList.RemoveAt(healthList.Count - 1);
        Destroy(heart);
    }

    public int collectedCount = 0;

    public void CollectItem(int amount)
    {
        collectedCount += amount;
        Debug.Log("Collected: " + collectedCount);
    }
}
