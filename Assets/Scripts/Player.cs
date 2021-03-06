﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Private Variables
    [SerializeField] private LayerMask platforms;

    private Rigidbody2D rigidBody;
    private BoxCollider2D playerCollider;
    private PlayerAnim anim;
    private GameObject sceneController;

    private Vector2 positionDashedFrom;
    private Vector2 dashEndPosition;
    private Queue<Vector2> previousPositions;

    private float[] movementSpeed;
    private float deltaTime;
    private float timeSinceLastDash;
    private float timeLeft;
    public float lockTime;
    
    private int numberOfDashes;

    public bool isDashing;
    private bool raycastHit;
    private bool attacking;

    private int health;

    // Pusblic Variables

    public int movementLevel;

    public bool killedAnEnemy;

    public Vector2 enemyPosition;

    public AudioSource soundPlayer;
    public AudioClip coinSound;
    public AudioClip wallSound;

    public bool Attacking
    {
        get { return attacking; }
    }

    public int NumberOfDashes
    {
        get { return numberOfDashes; }
    }
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public bool FacingRight
    {
        get { return rigidBody.velocity.x > 0; }
    }

    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = new float[] { 4.75f, 5.50f, 6.00f, 6.5f, 7.0f };

        deltaTime = 0.0f;
        movementLevel = 0;
        timeLeft = 0.2f;
        numberOfDashes = 3;

        health = 3;

        isDashing = false;
        raycastHit = false;
        attacking = false;

        previousPositions = new Queue<Vector2>();

        rigidBody = transform.GetComponent<Rigidbody2D>();
        playerCollider = transform.GetComponent<BoxCollider2D>();
        anim = transform.GetComponentInChildren<PlayerAnim>();

        sceneController = GameObject.Find("SceneController");

        Physics2D.gravity = new Vector2(0, -40.0f);
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        timeSinceLastDash += Time.deltaTime;
        lockTime += Time.deltaTime;

        if (sceneController.GetComponent<SceneManagement>().paused)
        {
            return;
        }

        Jump();
        Dash();
        if(!isDashing)
            Move();
        Attack();
        Death();

        if (rigidBody.transform.position.y < -80)
            health = 0;

        Camera.main.transform.position = new Vector3(rigidBody.transform.position.x, rigidBody.transform.position.y, -10.0f);

        anim.AnimUpdate(rigidBody.velocity.x, rigidBody.velocity.y, Grounded(), attacking);

        previousPositions.Enqueue(rigidBody.transform.position);

        if (previousPositions.Count > 10)
        {
            previousPositions.Dequeue();
        }

        ReplenishDash();
    }

    private void Jump()
    {
        float jumpValue = 5.0f;

        if (Grounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.velocity = Vector2.up * (jumpValue + (movementSpeed[movementLevel] * 2));
        }
    }

    private void Move()
    {
        float friction = 0.98f;

        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.velocity = new Vector2(-movementSpeed[movementLevel] * 2.5f, rigidBody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.velocity = new Vector2(movementSpeed[movementLevel] * 2, rigidBody.velocity.y);
        }
        else
        {
            if (Grounded())
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x * friction, rigidBody.velocity.y);

                if (rigidBody.velocity.x < 0.00001f && rigidBody.velocity.x > -0.00001f)
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }
        }
    }

    private void Dash()
    {
        // Loacal Variables
        float dashDistance = 2.75f;
        Vector2 playerPosition = rigidBody.transform.position;

        if (numberOfDashes > 0 && timeSinceLastDash > 0.75f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                soundPlayer.PlayOneShot(soundPlayer.clip);

                // Loacal Variables
                Vector2 mousePosition;
                Vector2 dashDirection;

                // Calculate mouse position && direction of the dash
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dashDirection = mousePosition - playerPosition;

                dashDirection.Normalize();

                // Calculate velocity
                rigidBody.velocity = new Vector2(dashDirection.x * movementSpeed[movementLevel] * 4, dashDirection.y * movementSpeed[movementLevel] * 4);

                // Calculate the end Position of the dash
                dashEndPosition = dashDirection * (dashDistance * (movementLevel + 1));

                dashEndPosition += playerPosition;

                // Ch
                RaycastHit2D raycast = Physics2D.Raycast(playerPosition, dashDirection, dashDistance * (movementLevel + 1), platforms);

                if (raycast.collider != null)
                {
                    raycastHit = true;
                    dashEndPosition = raycast.point;
                }

                positionDashedFrom = playerPosition;

                // Dash Physics
                isDashing = true;

                rigidBody.drag = 0;
                rigidBody.gravityScale = 0.0f;
                Physics2D.gravity = Vector2.zero;

                numberOfDashes--;

                if (numberOfDashes < 0)
                    numberOfDashes = 0;

                timeSinceLastDash = 0;
            }
        }

        Vector2 temp = positionDashedFrom - dashEndPosition;
        temp.Normalize();

        temp = temp * dashDistance * (movementLevel + 1);

        Debug.DrawLine(playerPosition, playerPosition - temp, Color.red);
        Debug.DrawLine(playerPosition, dashEndPosition, Color.white);
        Debug.DrawLine(Vector2.zero, dashEndPosition, Color.blue);

        float distance = Vector2.Distance(playerPosition, positionDashedFrom);

        float totalDistance = Vector2.Distance(dashEndPosition, positionDashedFrom);

        totalDistance = totalDistance - (raycastHit ? playerCollider.bounds.extents.x : 0);

        if(distance > totalDistance)
            Debug.Log(distance);
        Debug.Log(isDashing);

        if (isDashing)
        {
            if (HitWall())
                distance = totalDistance;

            if (distance >= totalDistance)
            {
                rigidBody.gravityScale = 0.97f;
                rigidBody.drag = 0.5f;
                Physics2D.gravity = new Vector2(0, -40.0f);

                isDashing = false;
                raycastHit = false;

                if (rigidBody.velocity.x < 0.00001f && rigidBody.velocity.x > -0.00001f)
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }
        }

        if (killedAnEnemy && (lockTime > 1.5f || PressedAKey()))
        {
            rigidBody.gravityScale = 0.98f;
            rigidBody.drag = 0.5f;
            Physics2D.gravity = new Vector2(0, -40.0f);

            isDashing = false;
            raycastHit = false;
            killedAnEnemy = false;

            lockTime = 0;
            numberOfDashes++;

            if (numberOfDashes > 3)
                numberOfDashes = 3;

            movementLevel++;

            if (movementLevel > 4)
                movementLevel = 4;
        }
        else if(killedAnEnemy && lockTime < 1.5f) 
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.transform.position = new Vector3(enemyPosition.x, enemyPosition.y + 0.4f);
            rigidBody.drag = 0;
            rigidBody.gravityScale = 0.0f;
            Physics2D.gravity = Vector2.zero;
        }
    }

    private bool Grounded()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0.0f, Vector2.down, 0.1f, platforms);

        return raycast.collider != null;
    }

    private bool HitWall()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0.0f, Vector2.left, 0.1f, platforms);

        if (raycast.collider != null)
            return true;

        raycast = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0.0f, Vector2.right, 0.1f, platforms);

        if (raycast.collider != null)
            return true;

        raycast = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0.0f, Vector2.up, 0.1f, platforms);

        if (raycast.collider != null)
            return true;

        return false;
    }

    private void ReplenishDash()
    {
        float dashCooldown = 3.5f;

        if (deltaTime > dashCooldown)
        {
            deltaTime = 0.0f;

            numberOfDashes++;

            if (numberOfDashes > 3)
                numberOfDashes = 3;
        }
    }
    private void ReplenishHealth()
    {
        float dashCooldown = 3.5f;

        if (deltaTime > dashCooldown)
        {
            deltaTime = 0.0f;

            numberOfDashes++;

            if (numberOfDashes > 3)
                numberOfDashes = 3;
        }
    }

    private bool IsStuck() 
    {
        Vector2 playerPosition = rigidBody.transform.position;

        foreach (Vector2 position in previousPositions)
        {
            if (position != playerPosition)
                return false;
        }

        return true;
    }

    private bool PressedAKey() 
    {
        return Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
    }

    private void Death()
    {
        if (health <= 0)
        {
            sceneController.GetComponent<SceneManagement>().completionStatus = 0;
            sceneController.GetComponent<SceneManagement>().ChangeScene("GameOverScene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // play pick up soin sound when hitting a coin
        if (collision.tag == "Coin")
        {
            soundPlayer.PlayOneShot(coinSound);
        }
        if (collision.tag == "WallTrigger" && attacking && movementLevel == 4)
        {
            soundPlayer.PlayOneShot(wallSound);
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attacking = true;
        }

        if (attacking)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                attacking = false;
                timeLeft = 0.2f;
            }
        }
    }
}
