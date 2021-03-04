using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTrigger(string trigger)
    {
        if (anim == null)
        {
            //anim = GetComponent<Animator>();
        }
        anim.SetTrigger(trigger);
    }

    public void SetBool(string name, bool value)
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }
        anim.SetBool(name, value);
    }
}
