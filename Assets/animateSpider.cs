using UnityEngine;

public class SpiderLegsSimple : MonoBehaviour
{
    public GameObject spider1; 
    public GameObject spider2; 
    public float swapInterval = 0.1f;

    private float timer = 0f;
    private bool isSpider1Active = true;

    void Awake()
    {
        if (spider1 == null || spider2 == null)
        {
            Debug.LogError("Assign spider1 and spider2 in the Inspector!");
            enabled = false;
            return;
        }

        spider1.SetActive(true);
        spider2.SetActive(false);
    }

    void Update()
    {
        bool isMoving =
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow);

        if (!isMoving)
        {
            spider1.SetActive(true);
            spider2.SetActive(false);
            timer = 0f;
            isSpider1Active = true;
            return;
        }

        timer += Time.deltaTime;
        if (timer >= swapInterval)
        {
            isSpider1Active = !isSpider1Active;
            spider1.SetActive(isSpider1Active);
            spider2.SetActive(!isSpider1Active);
            timer = 0f;
        }
    }
}


