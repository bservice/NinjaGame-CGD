using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;
    private Vector2 direction;

    private Player player;

    public float accelRate;
    public float maxSpeed;

    private float activationTime;
    private bool activated;

    public Vector2 Position
    {
        get { return position; }
        set
        {
            position = value;
        }
    }

    public Vector2 Direction
    {
        get { return direction; }
        set
        {
            direction = value;
        }
    }

    public float Y
    {
        get { return position.y; }
        set
        {
            position.y = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        activationTime = 0.5f;
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated)
        {
            activationTime -= Time.deltaTime;
            if (activationTime >= 0)
            {
                activated = true;
            }
        }

        transform.position = position;
        Shoot();
    }

    public void Shoot()
    {
        acceleration = accelRate * direction;

        velocity += acceleration;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        position += velocity;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && activated)
        {
            //Damage player here
            player.Health -= 1;
            Destroy(gameObject);
        }

        if(collision.tag == "Wall" || collision.tag == "Platform")
        {
            Destroy(gameObject);
        }
    }
}
