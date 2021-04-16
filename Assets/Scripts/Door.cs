using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private SceneManagement sceneController;

    // Start is called before the first frame update
    void Start()
    {
        sceneController = GameObject.Find("SceneController").GetComponent<SceneManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check to see if player has entered the door
        if (collision.tag == "Player")
        {
            sceneController.completionStatus = 1;
            sceneController.ChangeScene("GameOverScene");
        }
    }
}
