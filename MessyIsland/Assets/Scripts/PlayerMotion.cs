using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{ 
    CharacterController controller;
    Animator animator;
    [SerializeField] float speed;
    [SerializeField] float angularSpeed;
    float rx=0f,ry;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject grenade;
    AudioSource footStep;

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

    void generalMovementAnimations()
    {
        if (controller.velocity.magnitude > 0.1)
        {
            animator.SetBool("idle", false);
            if (Input.GetKey("a"))
            {
                animator.SetBool("left", true);
            }
            else
            {
                animator.SetBool("left", false);
            }
            if (Input.GetKey("d"))
            {
                animator.SetBool("right", true);
            }
            else
            {
                animator.SetBool("right", false);
            }
            if (Input.GetKey("w"))
            {
                animator.SetBool("forward", true);
            }
            else
            {
                animator.SetBool("forward", false);
            }
            if (Input.GetKey("s"))
            {
                animator.SetBool("backward", true);
            }
            else
            {
                animator.SetBool("backward", false);
            }
        }
        else
        {
            animator.SetBool("idle", true);         
        }
    }

    void pistolMovementAnimations()
    {
        if (controller.velocity.magnitude > 0.1)
        {
            animator.SetBool("idlePistol", false);
            if (Input.GetKey("a"))
            {
                animator.SetBool("leftPistol", true);
            }
            else
            {
                animator.SetBool("leftPistol", false);
            }
            if (Input.GetKey("d"))
            {
                animator.SetBool("rightPistol", true);
            }
            else
            {
                animator.SetBool("rightPistol", false);
            }
            if (Input.GetKey("w"))
            {
                animator.SetBool("forwardPistol", true);
            }
            else
            {
                animator.SetBool("forwardPistol", false);
            }
            if (Input.GetKey("s"))
            {
                animator.SetBool("backwardPistol", true);
            }
            else
            {
                animator.SetBool("backwardPistol", false);
            }
        }
        else
        {
            animator.SetBool("idlePistol", true);
        }
    }
}
