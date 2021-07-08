using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Humanoid
{
    public enum NPCAnimStatus { Idle, ForwardWalk, IdlePistol, ForwardPistolWalk, GrenadeThrow, Die };

    const float MAX_GRENADE_DISTANCE = 30.0f;
    const float MIN_GRENADE_DISTANCE = 10.0f;
    const float SPOT_DELAY = 1.5f;
    
    bool shoot;
    bool throwGrenade;
    bool isEnemySpotted;
    bool attacking;
 
    NavMeshAgent agent;
     
    [SerializeField] bool isLeader;

    [SerializeField] GameObject randomTarget;
    Transform weaponTarget = null;

    GameObject[] enemyTeam;
    GameObject spottedEnemy;

    GameObject rayOrigin;
      
    GameObject[] weapons;
    bool[] isWeaponTargeted;
  
    AudioSource footStep;

    void Start()
    {
        isNPC = true;
       
        weapons = roundManagerScript.getWeapons();
        
        isWeaponTargeted = new bool[weapons.Length];
        
        if (this.gameObject.tag == "Ninja")
        {
            enemyTeam = roundManagerScript.getSwatTeam();          
        }
        else 
        {
            enemyTeam = roundManagerScript.getNinjaTeam();        
        }

        rayOrigin = transform.GetChild(5).gameObject;

        agent = GetComponent<NavMeshAgent>();
        footStep = GetComponent<AudioSource>();   
           
    }

    void Update()
    {      
        if (agent.enabled && this.tag != "Dead")
        {               
            movementAnimations();            
            findNearestWeapon();          
            rayToEnemy();                                               
        }
    }

    void rayToEnemy()
    {              
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            if (enemyTeam[i] != null && enemyTeam[i].tag != "Dead")
            {                                      
                RaycastHit hit;
                float offset = 5.0f;
                Vector3 enemyPosition = new Vector3(enemyTeam[i].transform.position.x, enemyTeam[i].transform.position.y + offset, enemyTeam[i].transform.position.z);
                Vector3 direction = (enemyPosition - rayOrigin.transform.position).normalized;
                            
                if (Physics.Raycast(rayOrigin.transform.position, direction, out hit))
                {
                    Debug.DrawRay(rayOrigin.transform.position, direction * hit.distance, Color.yellow);
                    if (hit.transform.gameObject.name == enemyTeam[i].gameObject.name && enemyTeam[i].tag != "Dead")
                    {
                        Invoke("spotDelay", SPOT_DELAY);
                        float singleStep = 100.0f * Time.deltaTime;
                        Vector3 pistolDirection = Vector3.RotateTowards(pistol.transform.forward, direction, singleStep, 0.0f) * -1;
                        pistol.transform.rotation = Quaternion.LookRotation(pistolDirection);
                      
                        if (!attacking && enemyTeam[i].tag != "Dead")
                        {
                            //if (this.gameObject.tag == "Swat")
                            //{
                            //    Debug.Log("attacking");
                            //}
                            spottedEnemy = enemyTeam[i];
                            attack();
                        }                                                          
                    }
                    else 
                    {
                        isEnemySpotted = false;
                        shoot = false;
                        throwGrenade = false;                    
                        attacking = false;
                        followerLeader();
                
                        if ((this.gameObject.tag == "Ninja" && !roundManagerScript.teamHasMissingWeapons(RoundManager.TeamName.Ninja))
                            || (this.gameObject.tag == "Swat" && !roundManagerScript.teamHasMissingWeapons(RoundManager.TeamName.Swat)))
                        {
                        //    Debug.Log("Random Walk");                     
                            randomWalk();
                        }
                        else
                        {
                            if (weaponTarget != null)
                            {
                                if (isLeader)
                                {
                                    agent.SetDestination(weaponTarget.position);
                                    agent.stoppingDistance = 0;
                                }                         
                            }                           
                        }
                                                                           
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {  
        if (this.tag == "Ninja" && isAlive) 
        {
            if (other.tag == "SwatBullet") 
            {                     
                takeDamage(PistolBullet.DAMAGE, other.GetComponent<PistolBullet>().getOwner());              
            }              
        }
        else if (this.tag == "Swat" && isAlive)
        {
            if (other.tag == "NinjaBullet")
            {
                takeDamage(PistolBullet.DAMAGE, other.GetComponent<PistolBullet>().getOwner());
            }
        }      
    }

    void followerLeader()
    {
        if (!isLeader)
        {
            GameObject teamLeader;        
            if (this.gameObject.tag == "Ninja")
            {
                teamLeader = roundManagerScript.teamLeader(RoundManager.TeamName.Ninja);
            }
            else 
            {
                teamLeader = roundManagerScript.teamLeader(RoundManager.TeamName.Swat);
            }
            
            if (teamLeader != null)
            {              
                agent.SetDestination(teamLeader.transform.position);
                agent.stoppingDistance = 10;
            }
            else
            {
                this.isLeader = true;
            }
            
        }
    }

    void findNearestWeapon()
    {       
        if (isLeader && !attacking)
        {
            isWeaponTargetReached();
            if ((this.gameObject.tag == "Ninja" && roundManagerScript.teamHasMissingWeapons(RoundManager.TeamName.Ninja) && weaponTarget == null)
                || (this.gameObject.tag == "Swat" && roundManagerScript.teamHasMissingWeapons(RoundManager.TeamName.Swat) && weaponTarget == null))
            {
             //   Debug.Log("Searching for weapon");
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
        if (weaponTarget != null && agent.remainingDistance < 2)
        {
            weaponTarget = null;
        }
    }

    void randomWalk()
    {
        if (isLeader)
        {        
            agent.SetDestination(randomTarget.transform.position);
            agent.stoppingDistance = 0;
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

    void movementAnimations()
    {
        playFootStepSound();

        if (!isThrowingGrenadeAnim)
        {
            if (pistol.activeSelf)
            {                         
                pistolMovementAnimations();
            }
            else
            {
                generalMovementAnimations();
            }
        }
    }

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

    public void pickPistol()
    {
        setHasPistol(true);
        pistol.SetActive(true);
        animator.SetBool("isPistolActive", true);  
    }

    public void pickGrenade()
    {
        setHasGrenade(true);          
    }

    public bool getIsShooting()
    {
        return shoot;
    }

    public bool getIsThrowingGrenade()
    {
        return throwGrenade;
    }
  
    public void attack()
    {
        if (isEnemySpotted && spottedEnemy.tag != "Dead")
        {
            if (hasPistol || hasGrenade)
            {
               // Debug.Log("Attacking");
                attacking = true;
                agent.SetDestination(spottedEnemy.transform.position);
                agent.stoppingDistance = 10;             
                float distanceToEnemy = Vector3.Distance(transform.position, spottedEnemy.transform.position);
                if (hasPistol)
                {   
                    if (distanceToEnemy > MAX_GRENADE_DISTANCE || distanceToEnemy < MIN_GRENADE_DISTANCE
                        || !hasGrenade && distanceToEnemy <= MAX_GRENADE_DISTANCE && MIN_GRENADE_DISTANCE <= distanceToEnemy)
                    {
                        shoot = true;
                        throwGrenade = false;
                    }              
                }        
                if (hasGrenade)
                {   
                    if (distanceToEnemy <= MAX_GRENADE_DISTANCE && MIN_GRENADE_DISTANCE <= distanceToEnemy)
                    {
                        shoot = false;
                        throwGrenade = true;
                    }                    
                }            
            }
        }
        //else // No enemy is spotted
        //{
        //    shoot = false;
        //    throwGrenade = false;
        //    //currentEnemy = null;
        //    attacking = false;
        //    isEnemySpotted = false;
        //}

    }

    void spotDelay()
    {
        isEnemySpotted = true;
    }

    public override void takeDamage(float reduction, string enemyNickname)
    {
        base.takeDamage(reduction, enemyNickname);

        if (!isAlive)
        {
            isEnemySpotted = false;
            shoot = false;
            throwGrenade = false;
            attacking = false;
            animator.SetInteger("status", (int)NPC.NPCAnimStatus.Die);
            agent.enabled = false;
        }     
    }

}
