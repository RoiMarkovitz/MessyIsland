using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Humanoid
{
    
    void Start()
    {
             
    }

 
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (this.tag == "Ninja" && isAlive) // will be also "Swat"
        {
            if (other.tag == "SwatBullet") 
            {                     
                takeDamage(PistolBullet.DAMAGE, other.GetComponent<PistolBullet>().getOwner());             
            }              
        }
        
    }
   
}
