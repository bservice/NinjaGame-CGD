using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    private bool attack;
    private Enemy enemy;

    public bool Attack
    {
        get { return attack; }
    }
    // Start is called before the first frame update
    void Start()
    {
        attack = false;
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemy.autoShoot)
        {
            if (enemy.Left)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 0.0f);
            }
            else if (enemy.Right)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
