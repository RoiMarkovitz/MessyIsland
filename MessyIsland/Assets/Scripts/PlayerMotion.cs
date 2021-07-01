using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{ 
    public enum PlayerAnimStatus { Idle, ForwardWalk, RightWalk, LeftWalk, BackwardWalk, IdlePistol, ForwardPistolWalk,
    RightPistolWalk, LeftPistolWalk, BackwardPistolWalk, GrenadeThrow, Die}

    CharacterController controller;
    Animator animator;
    [SerializeField] float speed;
    [SerializeField] float angularSpeed;
    float rx=0f,ry;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject grenade;
    AudioSource footStep;

    bool isThrowingGrenade;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        footStep = GetComponent<AudioSource>();
      
    }

    void Update()
    {
        float dx, dz;
        // simple motion
        // transform.Translate(new Vector3(0, 0, 0.1f));

        // mouse input
        rx -= Input.GetAxis("Mouse Y") * angularSpeed * Time.deltaTime; // vertical rotation
        playerCamera.transform.localEulerAngles = new Vector3(rx, 0, 0);

        ry = transform.localEulerAngles.y+ Input.GetAxis("Mouse X") * angularSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, ry, 0); // runs on this (Player)
        // keyboard input
        dx = Input.GetAxis("Horizontal")*speed * Time.deltaTime;
        dz = Input.GetAxis("Vertical")*speed * Time.deltaTime;
        Vector3 motion = new Vector3(dx, -1, dz);
        motion = transform.TransformDirection(motion); // now in Global coordinates
        controller.Move(motion);
        
        playFootStepSound();
        movementAnimations();
        
    }

    void playFootStepSound()
    {
        if (!footStep.isPlaying && controller.velocity.magnitude > 0.1)
        {
            footStep.Play();
        }
    }

    void movementAnimations()
    {
        if (!isThrowingGrenade)
        { 
            if (pistol.activeSelf)
            {
                animator.SetBool("isPistolActive", true);
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

    void pistolMovementAnimations()
    {
        if (controller.velocity.magnitude > 0.1)
        {        
            if (Input.GetKey("a"))
            {
                animator.SetInteger("status", (int)PlayerAnimStatus.LeftPistolWalk);
            }         
            if (Input.GetKey("d"))
            {
                animator.SetInteger("status", (int)PlayerAnimStatus.RightPistolWalk);
            }
            if (Input.GetKey("w"))
            {
                animator.SetInteger("status", (int)PlayerAnimStatus.ForwardPistolWalk);
            }      
            if (Input.GetKey("s"))
            {
                animator.SetInteger("status", (int)PlayerAnimStatus.BackwardPistolWalk);
            }     
        }
        else
        {
            animator.SetInteger("status", (int)PlayerAnimStatus.IdlePistol);
        }
    }

    public void setIsThrowingGrenade(bool value)     
    {
        isThrowingGrenade = value;
    }
}
