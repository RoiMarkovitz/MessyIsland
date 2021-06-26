using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Humanoid
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

 
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.tag == "Ninja" && other.tag == "Swat")
        {
            animator.SetBool("isAlive", false);
        }
    }
}
