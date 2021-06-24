using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{ 
    private CharacterController controller;
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    private float rx=0f,ry;
    [SerializeField] private GameObject playerCamera;
    private AudioSource footStep;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
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
        if (!footStep.isPlaying && controller.velocity.magnitude > 0.1)
        {
            footStep.Play();
        }

    }
}
