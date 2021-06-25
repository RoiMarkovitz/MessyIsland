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

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        footStep = GetComponent<AudioSource>();
    }

    // Update is called once per frame
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
            animator.SetInteger("idle", -1);
            if (Input.GetKey("a"))
            {
                animator.SetInteger("left", 2);
            }
            else
            {
                animator.SetInteger("left", -1);
            }
            if (Input.GetKey("d"))
            {
                animator.SetInteger("right", 3);
            }
            else
            {
                animator.SetInteger("right", -1);
            }
            if (Input.GetKey("w"))
            {
                animator.SetInteger("forward", 1);
            }
            else
            {
                animator.SetInteger("forward", -1);
            }
            if (Input.GetKey("s"))
            {
                animator.SetInteger("backward", 4);
            }
            else
            {
                animator.SetInteger("backward", -1);
            }
        }
        else
        {
            animator.SetInteger("idle", 0);         
        }
    }

    void pistolMovementAnimations()
    {
        if (controller.velocity.magnitude > 0.1)
        {
            animator.SetInteger("idle", -1);
            if (Input.GetKey("a"))
            {
                animator.SetInteger("left", 2);
            }
            else
            {
                animator.SetInteger("left", -1);
            }
            if (Input.GetKey("d"))
            {
                animator.SetInteger("right", 3);
            }
            else
            {
                animator.SetInteger("right", -1);
            }
            if (Input.GetKey("w"))
            {
                animator.SetInteger("forward", 1);
            }
            else
            {
                animator.SetInteger("forward", -1);
            }
            if (Input.GetKey("s"))
            {
                animator.SetInteger("backward", 4);
            }
            else
            {
                animator.SetInteger("backward", -1);
            }
        }
        else
        {
            animator.SetInteger("idle", 0);
        }
    }
}
