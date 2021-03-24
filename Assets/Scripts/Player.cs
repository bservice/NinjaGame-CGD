using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platforms;

    private Rigidbody2D rigidBody;
    private BoxCollider2D playerCollider;

    private Vector2 positionDashedFrom;

    private float jumpValue;
    private float[] movementSpeed;
    private float friction;
    private float dashDistance;

    public int movementLevel;

    private bool stopFriction;

    private bool attacking;
    private float timeLeft;

    private PlayerAnim anim;

    private GameObject sceneController;

    public bool Attacking
    {
        get { return attacking; }
    }

    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = new float[] { 4.5f, 5.25f, 5.75f, 6.25f, 6.5f };
        friction = 0.98f;
        jumpValue = 6.0f;
        dashDistance = 1.0f;

        movementLevel = 0;

        stopFriction = false;

        attacking = false;
        timeLeft = 0.2f;

        rigidBody = transform.GetComponent<Rigidbody2D>();
        playerCollider = transform.GetComponent<BoxCollider2D>();
        anim = transform.GetComponent<PlayerAnim>();

        sceneController = GameObject.Find("SceneController");
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneController.GetComponent<SceneManagement>().paused)
        {
            return;
        }
        Vector3 newCamPos = new Vector3(rigidBody.transform.position.x, rigidBody.transform.position.y, -10);
        Camera.main.transform.position = newCamPos;
        Jump();
        Dash();
        Move();
        Attack();
        anim.AnimUpdate(rigidBody.velocity.x, rigidBody.velocity.y, Grounded());
        Debug.Log(rigidBody.velocity);
    }

    private void Jump()
    {
        if (Grounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.velocity = Vector2.up * jumpValue;
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.velocity = new Vector2(-movementSpeed[movementLevel], rigidBody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.velocity = new Vector2(movementSpeed[movementLevel], rigidBody.velocity.y);
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
        Vector2 playerPosition = rigidBody.transform.position;
        Vector2 normalizedDist = playerPosition + positionDashedFrom;
        normalizedDist.Normalize();

        float distance = Vector2.Distance(playerPosition, playerPosition + (normalizedDist * dashDistance));
        //float distance = Vector2.Distance(positionDashedFrom, playerPosition);

        if (distance > (dashDistance * movementLevel) && !stopFriction)
        {
            Debug.Log(dashDistance + "     " + distance);

            rigidBody.gravityScale = 1.0f;

            rigidBody.velocity = new Vector2(rigidBody.velocity.x * friction, rigidBody.velocity.y);

            if (rigidBody.velocity.x < 0.00001f && rigidBody.velocity.x > -0.00001f)
            {
                stopFriction = true;
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 normalizedVector = mousePosition - playerPosition;

            normalizedVector.Normalize();

            float movementX = normalizedVector.x * movementSpeed[movementLevel] * 6;
            float movementY = normalizedVector.y * movementSpeed[movementLevel] * 6;

            rigidBody.velocity = new Vector2(movementX, movementY);

            positionDashedFrom = playerPosition;

            stopFriction = false;

            rigidBody.gravityScale = 0.0f;            
        }
    }

    private bool Grounded()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0.0f, Vector2.down, 0.1f,  platforms);

        return raycast.collider != null;
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
