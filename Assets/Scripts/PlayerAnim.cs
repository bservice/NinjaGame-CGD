using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Anim animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Anim>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimUpdate(float xVelocity, float yVelocity, bool grounded)
    {
        animator.SetBool("Moving", xVelocity > 0);
    }
}
