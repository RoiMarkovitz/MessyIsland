using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Humanoid
{
    public enum NPCAnimStatus { Idle, ForwardWalk, IdlePistol, ForwardPistolWalk, GrenadeThrow, Die };

    bool isThrowingGrenade;

    NavMeshAgent agent;

    [SerializeField] bool isLeader;
    [SerializeField] GameObject target;
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject round;
    RoundManager roundManagerScript;

    AudioSource footStep;

    void Start()
    {
        isNPC = true;

        roundManagerScript = round.GetComponent<RoundManager>();

        agent = GetComponent<NavMeshAgent>();
        footStep = GetComponent<AudioSource>();
        // agent.Raycast
        // agent.stoppingDistance = 5; // Stop within this distance from the target position.
        // remainingDistance // The distance between the agent's position and the destination on the current path. (Read Only)
    }


    void Update()
    {
       if (agent.enabled && this.tag != "Dead")
       {         
            followerLeader();
            findNearestWeapon();
            
            // playFootStepSound();
            // movementAnimations();
            generalMovementAnimations();
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

    void followerLeader()
    {
        if (!isLeader)
        {
            GameObject ninjaLeader = roundManagerScript.getNinjaLeader();
            if (ninjaLeader != null)
            {
                agent.stoppingDistance = 3;
                agent.SetDestination(ninjaLeader.transform.position);
            }
            else
            {
                this.isLeader = true;
            }
            
        }
    }

    void findNearestWeapon()
    {
        if (isLeader)
        {
            if (roundManagerScript.teamHasMissingWeapons(RoundManager.TeamName.Ninja))
            {
                agent.stoppingDistance = 0;
                GameObject[] weapons = roundManagerScript.getWeapons();
                Transform newTarget = null;
                float currentMinDistance = 10000.0f;
                for (int i = 0; i< weapons.Length; i++)
                {
                    if (weapons[i].activeSelf)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, weapons[i].transform.position);
                        if (distanceToTarget < currentMinDistance)
                        {
                            currentMinDistance = distanceToTarget;
                            newTarget = weapons[i].transform;
                        }
                    }
                }
                if (newTarget != null)
                { 
                agent.SetDestination(newTarget.position);
                }
            }
        }
    }

    public NavMeshAgent getNavMeshAgent()
    {
        return agent;
    }

    void playFootStepSound()
    {
        if (!footStep.isPlaying && agent.velocity.magnitude > 0.1)
        {
            footStep.Play();
        }
    }

    //void movementAnimations()
    //{
    //    if (!isThrowingGrenade)
    //    {
    //        if (pistol.activeSelf)
    //        {
    //            animator.SetBool("isPistolActive", true);
    //            pistolMovementAnimations();

    //        }
    //        else
    //        {
    //            generalMovementAnimations();
    //        }
    //    }
    //}

    void generalMovementAnimations()
    {
        if (agent.velocity.magnitude > 0.1)
        {
            
            animator.SetInteger("status", (int)NPCAnimStatus.ForwardWalk);          
        }
        else
        {
            animator.SetInteger("status", (int)NPCAnimStatus.Idle);
        }
    }

    void pistolMovementAnimations()
    {
        if (agent.velocity.magnitude > 0.1)
        {         
            animator.SetInteger("status", (int)NPCAnimStatus.ForwardPistolWalk);        
        }
        else
        {
            animator.SetInteger("status", (int)NPCAnimStatus.IdlePistol);
        }
    }

    public void setIsThrowingGrenade(bool value)
    {
        isThrowingGrenade = value;
    }

}
