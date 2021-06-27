using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Humanoid
{
    Animator animator;
    
    NPC npcScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        npcScript = this.GetComponent<NPC>();
    }

 
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (this.tag == "Ninja") // will be also "Swat"
        {
  
            if (other.tag == "SwatBullet") // will be also "SwatGrenade"
            {
                Debug.Log("hit by bullet");
                npcScript.reduceHealth(PistolBullet.DAMAGE);
                Debug.Log(npcScript.getHealth().ToString());
            }

            isNpcDead();
               
        }
    }

    public void isNpcDead()
    {       
      if(!getIsAlive())
        { 
            animator.SetBool("isAlive", false);
        }

    }

    
   

   
}
