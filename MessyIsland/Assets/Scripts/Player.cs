using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    
    void Start()
    {
        isNPC = false;
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {

        if (this.tag == "Swat" && isAlive)
        {
            if (other.tag == "NinjaBullet")
            {
                takeDamage(PistolBullet.DAMAGE, other.GetComponent<PistolBullet>().getOwner());
                // red canvas
            }
        }

    }
}
