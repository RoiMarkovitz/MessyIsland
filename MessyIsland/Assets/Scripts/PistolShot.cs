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
    [SerializeField] GameObject shootingPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] ParticleSystem muzzleFlashParticles;

    AudioSource bulletSound;
    Animator animator;

    Humanoid userScript;
    NPC npcScript;


    void Start()
    {
        isShootingAllowed = true;
        bulletSound = GetComponent<AudioSource>();

        userScript = user.GetComponent<Humanoid>();
        isNPC = userScript.getIsNPC();

        if (isNPC)
        {
            npcScript = user.GetComponent<NPC>();
        }

        animator = user.GetComponent<Animator>();
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
        GameObject clonedBullet = Instantiate(bulletPrefab, shootingPoint.transform.position, shootingPoint.transform.rotation);
        clonedBullet.SetActive(true);
        clonedBullet.GetComponent<PistolBullet>().setOwner(userScript.getNickname());
        clonedBullet.tag = userScript.getNickname().Contains("Ninja") ? "NinjaBullet" : "SwatBullet";
        Vector3 velocity = transform.forward * BULLET_SPEED;
        Rigidbody rbClonedBullet = clonedBullet.GetComponent<Rigidbody>();
        rbClonedBullet.AddForce(velocity, ForceMode.Impulse);

        muzzleFlashParticles.Play();

        yield return new WaitForSeconds(SHOOT_DELAY);
        isShootingAllowed = true;
    }

    public void setIsShootingAllowed(bool val)
    {
        isShootingAllowed = val;
    }
}
