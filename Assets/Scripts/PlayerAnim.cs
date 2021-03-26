using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Anim animator;
    SpriteRenderer sprite;
    bool slashing;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Anim>();
        sprite = GetComponent<SpriteRenderer>();
        slashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimUpdate(float xVelocity, float yVelocity, bool grounded, bool attacking)
    {
        sprite.flipX = xVelocity < 0;

        if (attacking == slashing)
            return;

        if (attacking)
        {
            animator.ActivateTrigger("Attack");
        }
        else
        {
        animator.SetBool("Moving", Mathf.Abs(xVelocity) > 0);
        }
            slashing = attacking;
        
    }
}
