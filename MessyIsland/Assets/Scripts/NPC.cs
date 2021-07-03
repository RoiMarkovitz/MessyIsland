using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Humanoid
{
    public enum NPCAnimStatus { Idle, ForwardWalk, IdlePistol, ForwardPistolWalk, GrenadeThrow, Die };

    bool isThrowingGrenade;
    bool isWeaponTargetFound;

    NavMeshAgent agent;

    [SerializeField] bool isLeader;

    [SerializeField] GameObject randomTarget;
    Transform weaponTarget = null;
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject round;
    RoundManager roundManagerScript;
    GameObject[] weapons;
    bool[] isWeaponTargeted;
  
    AudioSource footStep;

    void Start()
    {
        isNPC = true;

        roundManagerScript = round.GetComponent<RoundManager>();
        weapons = roundManagerScript.getWeapons();
        isWeaponTargeted = new bool[weapons.Length];

        agent = GetComponent<NavMeshAgent>();
        footStep = GetComponent<AudioSource>();
        // agent.Raycast
           
    }


    void Update()
    {
       if (agent.enabled && this.tag != "Dead")
       {         
            followerLeader();
            findNearestWeapon();
            findEnemy();
            
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
                // stoppingDistance stops within this distance from the target position.
                agent.stoppingDistance = 5;
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
            isWeaponTargetReached();
            if (roundManagerScript.teamHasMissingWeapons(RoundManager.TeamName.Ninja) && weaponTarget == null)
            {
                int targetIndex = -1;
                float currentMinDistance = 10000.0f;
                for (int i = 0; i< weapons.Length; i++)
                {
                    if (weapons[i].activeSelf && !isWeaponTargeted[i])
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, weapons[i].transform.position);
                        if (distanceToTarget < currentMinDistance)
                        {
                            currentMinDistance = distanceToTarget;
                            weaponTarget = weapons[i].transform;
                            targetIndex = i;
                        }
                    }
                }
                if (weaponTarget != null)
                {
                    isWeaponTargeted[targetIndex] = true;
                    agent.SetDestination(weaponTarget.position);             
                }
            }
        }
    }

    void isWeaponTargetReached()
    {
        // remainingDistance is the distance between the agent's position and the destination on the current path. (Read Only)
        if (weaponTarget != null && agent.remainingDistance < 2)
        {
            weaponTarget = null;
        }
    }

    void findEnemy()
    {
        if (isLeader)
        {
            if (!roundManagerScript.teamHasMissingWeapons(RoundManager.TeamName.Ninja))
            {
                // Team has all weapons
                agent.SetDestination(randomTarget.transform.position);
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

    public bool isTargetReachable(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);
        if (path.status == NavMeshPathStatus.PathPartial)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
