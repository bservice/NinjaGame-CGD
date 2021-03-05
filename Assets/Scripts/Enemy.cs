using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private BoxCollider2D triggerBox;
    private Vector2 velocity;

    public float loc1;
    public float loc2;
    public float speed;

    private bool left;
    private bool right;
    private bool attack;

    private GameObject sceneController;

    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0, 1) == 1)
        { 
            right = true;
            left = false;
        }
        else
        {
            left = true;
            right = false;
        }
        attack = false;
        velocity = new Vector2();
        rigidBody = GetComponent<Rigidbody2D>();
        triggerBox = GetComponentInChildren<BoxCollider2D>();

        sceneController = GameObject.Find("SceneController");
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneController.GetComponent<SceneManagement>().paused)
        {
            return;
        }

        MoveBackAndForth();

        rigidBody.position += velocity * Time.deltaTime;
        velocity = Vector2.zero;
    }

    public void MoveBackAndForth()
    {
        if (right)
        {
            if (rigidBody.position.x >= loc1)
            {
                right = !right;
                left = !left;
            }
            velocity = new Vector2(1, 0) * speed;
        }
        if (left)
        {
            if (rigidBody.position.x <= loc2)
            {
                right = !right;
                left = !left;
            }
            velocity = new Vector2(-1, 0) * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            attack = true;
            Debug.Log("attack now");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            attack = false;
            Debug.Log("can't attack");
        }
    }
}
