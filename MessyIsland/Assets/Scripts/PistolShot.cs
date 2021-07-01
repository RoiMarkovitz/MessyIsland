using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShot : MonoBehaviour
{
    const float SHOOT_DELAY = 0.5f;
    const float BULLET_SPEED = 400.0f;

    [SerializeField] GameObject player;
    GameObject bullet;
    ParticleSystem muzzleFlashParticles;

    AudioSource bulletSound;
    Animator animator;

    [SerializeField] GameObject playerCamera;
    Player playerScript;

    bool isShootingAllowed;
    
    
    void Start()
    {
        bulletSound = GetComponent<AudioSource>();
        bullet = this.transform.GetChild(2).gameObject;
        muzzleFlashParticles = this.transform.GetChild(3).GetComponent<ParticleSystem>(); ;
        playerScript = player.GetComponent<Player>();
        animator = player.GetComponent<Animator>();
        isShootingAllowed = true;
    }

    
    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && this.gameObject.activeSelf
        //    && isShootingAllowed && playerScript.getHasPistol()
        //    && animator.GetInteger("status") != (int)PlayerMotion.PlayerAnimStatus.GrenadeThrow)
        //{       
        //    StartCoroutine(shoot());
        //}
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && this.gameObject.activeSelf
          && isShootingAllowed && playerScript.getHasPistol()
          && animator.GetInteger("status") != (int)PlayerMotion.PlayerAnimStatus.GrenadeThrow)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        isShootingAllowed = false;
        bulletSound.Play();
        GameObject clonedBullet = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation);
        clonedBullet.SetActive(true);
        clonedBullet.GetComponent<PistolBullet>().setOwner(playerScript.getNickname());

        Rigidbody rbClonedBullet = clonedBullet.GetComponent<Rigidbody>();
        rbClonedBullet.AddForce(playerCamera.transform.forward * BULLET_SPEED, ForceMode.Impulse);

        muzzleFlashParticles.Play();
       
        yield return new WaitForSeconds(SHOOT_DELAY);

        isShootingAllowed = true;
        
    }

}
