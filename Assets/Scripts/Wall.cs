using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private Player player;
    private GameObject sceneController;

    //Bool that tests if wall is eligible to be attacked
    private bool attack;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        sceneController = GameObject.Find("SceneController");
        Debug.Log(GetComponent<BoxCollider2D>().tag);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDestroy();
    }

    private void CheckForDestroy()
    {
        //Is the wall eligible to be attacked and is the player in an attacking state?
        //*****Can add more requirements for destroy like a speed threshhold here
        if (attack && player.isDashing && player.movementLevel == 4)
        {
            player.movementLevel = 0;
            sceneController.GetComponent<SceneManagement>().AddScore(10);
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Mark wall as eligible to be destroyed only if the player is within the kill bounds
        if (collision.tag == "Player")
        {
            attack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            attack = false;
        }
    }
}
