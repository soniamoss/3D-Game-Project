using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // Needed for Text UI

public class SpiderDiner : MonoBehaviour
{
    [Header("Health Settings")]
    public GameObject healthPrefab;
    public int maxNumLives = 3;

    [Header("Layout")]
    public float spacing = 50f;

    private List<GameObject> healthList = new List<GameObject>();
   
    [Header("Win Condition")]
    public int totalFlies;          // how many flies exist in this level
    private int collectedFlies = 0;

    [Header("Fly UI")]
    public Text fliesText;          // drag your "Flies Left" Text here

    void Start()
    {
        CreateHealthBar();
        UpdateFlyUI();              // make sure UI starts in correct state
        UpdateFlyUI();          // update when a new fly is registered
    }

    public void FlyCollected()
    {
        collectedFlies++;
        UpdateFlyUI();          // update when a fly is collected

        if (collectedFlies >= totalFlies && totalFlies > 0)
        {
            WinGame();
        }
    }

    void UpdateFlyUI()
    {
        if (fliesText != null)
        {
            int remaining = Mathf.Max(totalFlies - collectedFlies, 0);
            fliesText.text = "Flies Left: " + remaining;
        }
    }

    void WinGame()
    {
        Debug.Log("YOU WIN!");

        // Optional:
        Time.timeScale = 0f; // pause game
        // show win UI here (panel, button to main menu, etc.)
    }
}
