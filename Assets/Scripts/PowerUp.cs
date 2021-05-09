using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameObject sceneController;
    private Player player;
    private float timePassed;
    private float powerUpLength;
    private bool pickedUp;
    private int speedChange;

    // Start is called before the first frame update
    void Start()
    {
        sceneController = GameObject.Find("SceneController");
        player = GameObject.Find("Player").GetComponent<Player>();
        timePassed = 0;
        pickedUp = false;
        speedChange = 2;

        switch (gameObject.tag)
        {
            case "DoubleScore":
                powerUpLength = 5;
                break;
            case "SpeedBoost":
                powerUpLength = 5;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp)
        {
            timePassed += Time.deltaTime;
        }
        
        if (timePassed >= powerUpLength)
        {
            switch (gameObject.tag)
            {
                case "DoubleScore":
                    sceneController.GetComponent<SceneManagement>().scoreModifier = 1;
                    break;
                case "SpeedBoost":
                    player.movementLevel -= speedChange;
                    if (player.movementLevel < 0)
                    {
                        player.movementLevel = 0;
                    }
                    break;
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !pickedUp)
        {
            switch (gameObject.tag)
            {
                case "DoubleScore":
                    sceneController.GetComponent<SceneManagement>().scoreModifier = 2;
                    break;
                case "SpeedBoost":
                    player.movementLevel += speedChange;
                    if (player.movementLevel > 4)
                    {
                        player.movementLevel = 4;
                    }
                    break;
            }
            pickedUp = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
