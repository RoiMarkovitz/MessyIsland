using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Humanoid
{
    public enum NPCAnimStatus { Idle, ForwardWalk, IdlePistol, ForwardPistolWalk, GrenadeThrow, Die };

    const float MAX_GRENADE_DISTANCE = 60.0f;
    const float MIN_GRENADE_DISTANCE = 30.0f;

    bool shoot;
    bool throwGrenade;
    bool currentEnemy;
    bool attacking;

    NavMeshAgent agent;

    [SerializeField] private LayerMask enemyMask;
    [SerializeField] bool isLeader;

    [SerializeField] GameObject randomTarget;
    Transform weaponTarget = null;

    GameObject[] enemyTeam;
    GameObject spottedEnemy;
    GameObject current_enemy;

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

        isCeasefireTime();
    }

    void rayToEnemy()
    {
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            if (enemyTeam[i] != null && enemyTeam[i].tag != "Dead")
            {
                if (currentEnemy)
                {
                    current_enemy = spottedEnemy;
                }
                else
                {
                    current_enemy = enemyTeam[i];
                }
                
                RaycastHit hit;
                float offset = 5.0f;
                Vector3 enemyPosition = new Vector3(current_enemy.transform.position.x, current_enemy.transform.position.y + offset, current_enemy.transform.position.z);
                Vector3 direction = (enemyPosition - rayOrigin.transform.position).normalized;

                if (Physics.Raycast(rayOrigin.transform.position, direction, out hit, Mathf.Infinity, enemyMask.value))
                {
                    Debug.DrawRay(rayOrigin.transform.position, direction * hit.distance, Color.yellow);
              
                    if (hit.transform.gameObject.name == current_enemy.gameObject.name && current_enemy.tag != "Dead")
                    {
                        float singleStep = 90.0f * Time.deltaTime * Mathf.Deg2Rad;
                        if (hasPistol)
                        {
                            Vector3 pistolDirection = Vector3.RotateTowards(pistol.transform.forward, direction, singleStep, 0.0f);
                            pistol.transform.rotation = Quaternion.LookRotation(pistolDirection);
                        }
                        if (hasPistol || hasGrenade)
                        {
                            spottedEnemy = current_enemy;
                            agent.SetDestination(spottedEnemy.transform.position);
                            currentEnemy = true;
                            attack();
                        }
                        else
                        {
                            followerLeader();
                        }
                    }
                    else
                    {
                        followerLeader();
                        shoot = false;
                        throwGrenade = false;
                        attacking = false;
                        currentEnemy = false;

                        if (gameObject.CompareTag("Ninja") && !roundManagerScript.teamHasMissingWeapons(RoundManager.TeamName.Ninja)
                            || gameObject.CompareTag("Swat") && !roundManagerScript.teamHasMissingWeapons(RoundManager.TeamName.Swat))
                        {
                            randomWalk();
                        }
                        else
                        {
                            if (weaponTarget)
                            {
                                if (isLeader)
                                {
                                    agent.SetDestination(weaponTarget.position);
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
                if (!currentEnemy)
                {
                    agent.SetDestination(teamLeader.transform.position);
                    if (this.gameObject.tag == "Ninja")
                    {
                        agent.stoppingDistance = 5;
                    }
                    else
                    {
                        agent.stoppingDistance = 10;
                    }
                }
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
                int targetIndex = -1;
                float currentMinDistance = 10000.0f;
                for (int i = 0; i < weapons.Length; i++)
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
        if (hasPistol || hasGrenade)
        {
            if (spottedEnemy.tag != "Dead")
            {
                attacking = true;
                float distanceToEnemy = Vector3.Distance(transform.position, spottedEnemy.transform.position);
                
                if (hasPistol && !hasGrenade)
                {
                    throwGrenade = false;
                    shoot = true;              
                }
                
                if (hasGrenade && !hasPistol)
                {
                    if (distanceToEnemy <= MAX_GRENADE_DISTANCE)
                    {
                        throwGrenade = true;
                        shoot = false;
                    }
                }
                if (hasPistol && hasGrenade)
                {
                    if (distanceToEnemy <= MAX_GRENADE_DISTANCE && MIN_GRENADE_DISTANCE <= distanceToEnemy)
                    {
                        throwGrenade = true;
                        shoot = false;
                    }
                    else
                    {
                        shoot = true;
                        throwGrenade = false;

                    }
                }
                
            }
        }
    }

    public override void takeDamage(float reduction, string enemyNickname)
    {
        base.takeDamage(reduction, enemyNickname);

        if (!isAlive)
        {
            shoot = false;
            throwGrenade = false;
            attacking = false;
            currentEnemy = false;
            animator.SetInteger("status", (int)NPC.NPCAnimStatus.Die);
            agent.enabled = false;
        }
    }

    public void isCeasefireTime()
    {
        int count = 0;
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            if (enemyTeam[i].tag == "Dead")
            {
                count++;
            }
        }

        if (count == enemyTeam.Length)
        {
            shoot = false;
            throwGrenade = false;
        }
    }

}
