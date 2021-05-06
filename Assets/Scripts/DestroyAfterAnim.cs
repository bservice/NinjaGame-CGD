using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnim : MonoBehaviour
{
    [SerializeField]
    private AnimationClip animation;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, animation.length - .2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
