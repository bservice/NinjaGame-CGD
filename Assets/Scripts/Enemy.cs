using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRender;
    private Vector2 velocity;

    public float rightPos;
    public float leftPos;
    public float speed;

    private bool left;
    private bool right;
    private bool attack;
    private bool dead;

    private GameObject sceneController;

    private Player player;

    private EnemyAnim anim;

    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0, 2) == 1)
        {
            right = true;
            left = false;
        }
        else
        {
            left = true;
            right = false;
        }
        dead = false;
        attack = false;
        velocity = new Vector2();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();

        sceneController = GameObject.Find("SceneController");

        player = FindObjectOfType<Player>();

        anim = GetComponentInChildren<EnemyAnim>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneController.GetComponent<SceneManagement>().paused)
        {
            return;
        }

        //Check to see if the enemy is being killed
        CheckForKill();

        //If enemy is alive, move it back and forth
        if (!dead)
        {
            MoveBackAndForth();
        }


        rigidBody.position += velocity * Time.deltaTime;

        anim.AnimUpdate(velocity.x);

        velocity = Vector2.zero;
    }

    public void MoveBackAndForth()
    {
        if (right)
        {
            if (rigidBody.position.x >= rightPos)
            {
                right = !right;
                left = !left;
            }
            velocity = new Vector2(1, 0) * speed;
        }
        if (left)
        {
            if (rigidBody.position.x <= leftPos)
            {
                right = !right;
                left = !left;
            }
            velocity = new Vector2(-1, 0) * speed;
        }
    }

    private void CheckForKill()
    {
        //Is the enemy eligible to be attacked and is the player in an attacking state?
        //*****Can add more requirements for destroy like a speed threshhold here
        if (attack && player.Attacking)
        {
            spriteRender.color = Color.red;
            dead = true;
            if (player.movementLevel < 4)
            {
                player.movementLevel += 1;
            }
            sceneController.GetComponent<SceneManagement>().score += 100;
            Destroy(this);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Mark enemy as eligible to be killed only if the player is within the kill bounds
        if(collision.tag == "Player")
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
