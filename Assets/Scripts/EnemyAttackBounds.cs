using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBounds : MonoBehaviour
{
    private bool attack;

    public bool Attack
    {
        get { return attack; }
        set { attack = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Mark enemy as eligible to be killed only if the player is within the kill bounds
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
