using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyCollect : MonoBehaviour
{
    //Add the values for fly counter and starting values
    private SpiderDiner gameManager;
    public int value = 1;
    float startY;

    //Starting here we should be able to collect the flies from arounf the map
    void Start()
    {
        gameManager = FindObjectOfType<SpiderDiner>();

        //This shows that the fly exists
        if (gameManager != null)
        {
            gameManager.RegisterFly();
        }

        //Remember starting height for bobbing
        startY = transform.position.y;
    }

    void Update()
    {
        //Simple bobbing up and down animation
        transform.position = new Vector3(
            transform.position.x,
            startY + Mathf.Sin(Time.time * 2f) * 0.25f,
            transform.position.z
        );
    }

    void OnTriggerEnter(Collider other)
    {
        //Only the spider can collect and interact with the fly
        if (!other.CompareTag("Player")) return;

        //Show fly collection and health gain
        if (gameManager != null)
        {
            gameManager.FlyCollected();
          
            gameManager.GainHealth(value);

        }

        //remove fly from the scene
        Destroy(gameObject);
    }
}



