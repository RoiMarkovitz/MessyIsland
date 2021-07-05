using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    public enum PlayerAnimStatus
    {
        Idle, ForwardWalk, RightWalk, LeftWalk, BackwardWalk, IdlePistol, ForwardPistolWalk,
        RightPistolWalk, LeftPistolWalk, BackwardPistolWalk, GrenadeThrow, Die
    }

    CharacterController controller;
    
    [SerializeField] float speed;
    [SerializeField] float angularSpeed;
    float rx = 0f, ry;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject takeDamagePanel;

    AudioSource footStep;

    
    void Start()
    {
        isNPC = false;
        controller = GetComponent<CharacterController>();
       
        footStep = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        float dx, dz;

        // mouse input
        rx -= Input.GetAxis("Mouse Y") * angularSpeed * Time.deltaTime; // vertical rotation
        // use Clampf to limit the sight angles
        playerCamera.transform.localEulerAngles = new Vector3(rx, 0, 0);

        ry = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * angularSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, ry, 0); // runs on this (Player)
        // keyboard input
        dx = Input.GetAxis("Horizontal") * speed * Time.deltaTime; // Horizontal means 'A' or 'D'
        dz = Input.GetAxis("Vertical") * speed * Time.deltaTime; // Verticalal means 'W' or 'S'
        Vector3 motion = new Vector3(dx, -1, dz); // in local coordinates
        motion = transform.TransformDirection(motion); // in Global coordinates
        controller.Move(motion);

        
        movementAnimations();
    }

    void OnTriggerEnter(Collider other)
    {

        if (this.tag == "Swat" && isAlive)
        {
            if (other.tag == "NinjaBullet")
            {
                takeDamage(PistolBullet.DAMAGE, other.GetComponent<PistolBullet>().getOwner());               
            }
        }

    }

    void playFootStepSound()
    {
        if (!footStep.isPlaying && controller.velocity.magnitude > 0.1)
        {
            footStep.Play();
        }
    }

    public override void takeDamage(float reduction, string enemyNickname)
    {
        takeDamagePanel.SetActive(true);
        Invoke("endTakeDamageEffect", 1.0f);
    }

    void endTakeDamageEffect()
    {
        takeDamagePanel.SetActive(false);
    }

    void movementAnimations()
    {
        playFootStepSound();

        if (!isThrowingGrenadeAnim)
        {
            generalMovementAnimations();
            //if (pistol.activeSelf)
            //{
            //    animator.SetBool("isPistolActive", true);
            //    pistolMovementAnimations();

            //}
            //else
            //{
            //    generalMovementAnimations();
            //}
        }
    }

    void generalMovementAnimations()
    {
        if (controller.velocity.magnitude > 0.1)
        {
            if (Input.GetKey("a"))
            {
                animator.SetInteger("status", (int)PlayerAnimStatus.LeftWalk);
            }
            if (Input.GetKey("d"))
            {
                animator.SetInteger("status", (int)PlayerAnimStatus.RightWalk);
            }
            if (Input.GetKey("w"))
            {
                animator.SetInteger("status", (int)PlayerAnimStatus.ForwardWalk);
            }
            if (Input.GetKey("s"))
            {
                animator.SetInteger("status", (int)PlayerAnimStatus.BackwardWalk);
            }

        }
        else
        {
            animator.SetInteger("status", (int)PlayerAnimStatus.Idle);
        }
    }

    //void pistolMovementAnimations()
    //{
    //    if (controller.velocity.magnitude > 0.1)
    //    {        
    //        if (Input.GetKey("a"))
    //        {
    //            animator.SetInteger("status", (int)PlayerAnimStatus.LeftPistolWalk);
    //        }         
    //        if (Input.GetKey("d"))
    //        {
    //            animator.SetInteger("status", (int)PlayerAnimStatus.RightPistolWalk);
    //        }
    //        if (Input.GetKey("w"))
    //        {
    //            animator.SetInteger("status", (int)PlayerAnimStatus.ForwardPistolWalk);
    //        }      
    //        if (Input.GetKey("s"))
    //        {
    //            animator.SetInteger("status", (int)PlayerAnimStatus.BackwardPistolWalk);
    //        }     
    //    }
    //    else
    //    {
    //        animator.SetInteger("status", (int)PlayerAnimStatus.IdlePistol);
    //    }
    //}

    
}
