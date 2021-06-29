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
        
        if (this.tag == "Ninja" && isAlive) // will be also "Swat"
        {
  
            if (other.tag == "SwatBullet") // will be also "SwatGrenade"
            {
                Debug.Log("hit by bullet");
                npcScript.takeDamage(PistolBullet.DAMAGE);
                Debug.Log(npcScript.getCurrentHealth().ToString());
            }

            isNpcDead();
               
        }
        
    }

    public void isNpcDead()
    {       
      if(!getIsAlive())
        {
            this.gameObject.tag = "Dead";
            isAlive = false;
            animator.SetBool("isAlive", false);
        }

    }

    
   

   
}
