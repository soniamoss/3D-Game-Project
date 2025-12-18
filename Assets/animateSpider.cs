using UnityEngine;

//This script should transform the spider into a animation
public class SpiderLegsSimple : MonoBehaviour
{
    //Declare values and get both spider objects to combine
    public GameObject spider1; 
    public GameObject spider2; 
    public float swapInterval = 0.1f;

    private float timer = 0f;
    private bool isSpider1Active = true;

    //We should only have one active spider at a time
    void Awake()
    {
        if (spider1 == null || spider2 == null)
        {
            Debug.LogError("Assign spider1 and spider2 in the Inspector!");
            enabled = false;
            return;
        }
        //if spider 1 is active then set spider2 to inactive
        spider1.SetActive(true);
        spider2.SetActive(false);
    }

    //Update the movements with the arrow key inputs
    void Update()
    {
        bool isMoving =
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow);

        //Update current moving status
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




