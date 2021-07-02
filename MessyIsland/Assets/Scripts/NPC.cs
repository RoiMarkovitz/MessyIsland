using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Humanoid
{
    NavMeshAgent agent;

    [SerializeField] bool isLeader;
    [SerializeField] GameObject target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

 
    void Update()
    {
        if (agent.enabled)
        {
           agent.SetDestination(target.transform.position);
        }
        
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

    public NavMeshAgent getNavMeshAgent()
    {
        return agent;
    }

}
