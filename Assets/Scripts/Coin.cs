using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameObject sceneController;

    // Start is called before the first frame update
    void Start()
    {
        sceneController = GameObject.Find("SceneController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // increase the score when the player touches a coin and destroy the coin
        if (collision.tag == "Player")
        {
            sceneController.GetComponent<SceneManagement>().AddScore(50);
            Destroy(gameObject);
        }
    }
}
