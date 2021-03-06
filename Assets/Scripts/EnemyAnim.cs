﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    SpriteRenderer sprite;
    Anim anim;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Anim>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimUpdate(float xVelocity)
    {
        sprite.flipX = xVelocity < 0;
    }
    public void Shoot(bool value)
    {
        anim.SetBool("Shoot", value);
    }
}
