using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShot : MonoBehaviour
{
    const float SHOOT_DELAY = 0.5f;
    const float BULLET_SPEED = 400.0f;

    bool isShootingAllowed;
    bool isNPC;

    [SerializeField] GameObject user;
    GameObject bullet;
    ParticleSystem muzzleFlashParticles;

    AudioSource bulletSound;
    Animator animator;

    Humanoid userScript;
    NPC npcScript;
    

    void Start()
    {
        isShootingAllowed = true;
        bulletSound = GetComponent<AudioSource>();
        bullet = this.transform.GetChild(2).gameObject;
        muzzleFlashParticles = this.transform.GetChild(3).GetComponent<ParticleSystem>();

        userScript = user.GetComponent<Humanoid>();
        isNPC = userScript.getIsNPC();

        if (isNPC)
        {
            npcScript = user.GetComponent<NPC>();
        }

        animator = user.GetComponent<Animator>();      
    }

    
    void Update()
    {
      
    }

    void FixedUpdate()
    {
        if (isShootingAllowed && userScript.getHasPistol() && this.gameObject.activeSelf)
        {
            if (isNPC)
            {        
                if (npcScript.getIsShooting() && animator.GetInteger("status") != (int)NPC.NPCAnimStatus.GrenadeThrow)
                {
                    
                    StartCoroutine(shoot());
                }
               
            }
            else if (Input.GetMouseButtonDown(0) && animator.GetInteger("status") != (int)Player.PlayerAnimStatus.GrenadeThrow)
            {
                StartCoroutine(shoot());
            }
        }
                    
    }
    
    IEnumerator shoot()
    {
        isShootingAllowed = false;
        bulletSound.Play();
        GameObject clonedBullet = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation);
        clonedBullet.SetActive(true);
        clonedBullet.GetComponent<PistolBullet>().setOwner(userScript.getNickname());

        Vector3 velocity = (transform.forward * BULLET_SPEED) * -1;

        Rigidbody rbClonedBullet = clonedBullet.GetComponent<Rigidbody>();
        rbClonedBullet.AddForce(velocity, ForceMode.Impulse);

        muzzleFlashParticles.Play();
       
        yield return new WaitForSeconds(SHOOT_DELAY);

        isShootingAllowed = true;
        
    }

    

}
