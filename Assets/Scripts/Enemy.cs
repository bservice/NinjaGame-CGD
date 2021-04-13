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

    //Booleans to turn off and on auto shoot and how long they should shoot for
    public bool autoShoot;
    public float shootTime;
    private float firstShootTime;

    private GameObject sceneController;

    private Player player;

    private EnemyAnim anim;

    public Bullet bulletPrefab;
    private Bullet bullet;

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

        firstShootTime = shootTime;

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

        //Only allow auto shooting if it is turned on
        if (autoShoot)
        {
            shootTime -= Time.deltaTime;
            if (shootTime <= 0.0f)
            {
                Shoot();
                shootTime = firstShootTime;
            }
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
                player.killedAnEnemy = true;
                player.enemyPosition = rigidBody.transform.position;
                player.lockTime = 0;
            }
            sceneController.GetComponent<SceneManagement>().score += 100;
            Destroy(this);
            Destroy(this.gameObject);
        }
    }

    //Method to create and shoot bullets
    private void Shoot()
    {
        bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        if (bullet != null)
        {
            
            if (right)
            {
                bullet.Direction = new Vector2(1.0f, 0.0f);
            }
            if (left)
            {
                bullet.Direction = new Vector2(-1.0f, 0.0f);
            }
            bullet.Position = transform.position;
            bullet.Y = transform.position.y + 0.478f;
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
