using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpiderDiner : MonoBehaviour
{
    //Adding in health to increase and decrease health counter(hearts)
    [Header("Health Settings")]
    public GameObject healthPrefab;
    public int maxNumLives = 3;

    [Header("Layout")]
    public float spacing = 50f;

    private List<GameObject> healthList = new List<GameObject>();
    
    [Header("Win Condition")]

    //Use total flies for fly count down(reaching 0 ends game)
    public int totalFlies;
    private int collectedFlies = 0;

    [Header("Fly UI")]

    //Text that adds the fly count
    public TextMeshProUGUI fliesText;

    //Add in a win screen for the end(all flies collected)

    [Header("Win Screen")]
    public GameObject winScreen;

    void Start()
    {
        CreateHealthBar();
        UpdateFlyUI();

        //make sure win screen starts hidden
        if (winScreen != null)
            winScreen.SetActive(false);
    }

    void CreateHealthBar()
    {
        for (int i = 0; i < maxNumLives; i++)
        {
            GameObject heart = Instantiate(healthPrefab, transform);

            RectTransform rt = heart.GetComponent<RectTransform>();

            //Anchor to top-right
            rt.anchorMin = new Vector2(1, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(1, 1);

            //Position from top-right corner
            rt.anchoredPosition = new Vector2(i * spacing, 0);

            healthList.Add(heart);
        }
    }

    //when player takes damage from shoe
    public void LoseHealth()
    {
        if (healthList.Count == 0) return;

        GameObject heart = healthList[healthList.Count - 1];
        healthList.RemoveAt(healthList.Count - 1);
        Destroy(heart);
    }

    public void GainHealth(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            //Make sure maxlives is set to max of 3
            if (healthList.Count >= maxNumLives)
                return;

            GameObject heart = Instantiate(healthPrefab, transform);

            RectTransform rt = heart.GetComponent<RectTransform>();

            //Anchor to top-right
            rt.anchorMin = new Vector2(1, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(1, 1);

            //Add new heart and position it based on which life we are on
            int index = healthList.Count;
            rt.anchoredPosition = new Vector2(index * spacing, 0);

            healthList.Add(heart);
        }
    }

    //Use this as a fly counter for flies consumed
    public void RegisterFly()
    {
        totalFlies++;
        UpdateFlyUI();
    }

    public void FlyCollected()
    {
        collectedFlies++;
        UpdateFlyUI();

        if (collectedFlies >= totalFlies && totalFlies > 0)
        {
            WinGame();
        }
    }

    //Update the text to show accurate num of flies left
    void UpdateFlyUI()
    {
        if (fliesText != null)
        {
            int remaining = Mathf.Max(totalFlies - collectedFlies, 0);
            fliesText.text = "Flies Left: " + remaining;
        }
    }

    //Run this if all the flies are collected to show winscreen
    void WinGame()
    {
        Debug.Log("YOU WIN!");

        //Pause the game
        Time.timeScale = 0f;

        //Show the win screen panel
        if (winScreen != null)
            winScreen.SetActive(true);
    }
}





