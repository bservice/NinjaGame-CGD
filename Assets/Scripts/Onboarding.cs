using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onboarding : MonoBehaviour
{
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //text.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            text.SetActive(true);
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.SetActive(false);
        }
    }
}
