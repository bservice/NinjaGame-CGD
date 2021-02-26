using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector2 velocity;
    private Vector2 acceleration;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector2();
        acceleration = new Vector2();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //rigidBody.AddForce(new Vector2(0, -1));
        if (Input.GetKeyDown(KeyCode.W))
        {
            //rigidBody.AddForce(new Vector2(0, 30));
            //velocity = new Vector2(0, 300);
            rigidBody.position += (new Vector2(0, 6));
        }
        if (Input.GetKey(KeyCode.A))
        {
            //rigidBody.AddForce(new Vector2(-6, 0) );
            velocity = new Vector2(-6, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //rigidBody.AddForce(new Vector2(6, 0) * Time.deltaTime);
            velocity = new Vector2(6, 0);
        }

        rigidBody.position += velocity * Time.deltaTime;
        velocity = Vector2.zero;
    }
}
