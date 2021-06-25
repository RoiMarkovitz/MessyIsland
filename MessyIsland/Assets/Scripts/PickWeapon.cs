using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickWeapon : MonoBehaviour
{
    [SerializeField] GameObject fireCrosshair;
    [SerializeField] GameObject actionCrosshair;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject playerPistol;
    [SerializeField] GameObject playerGrenade;
    bool isTriggerHit;


    void Start()
    {
        isTriggerHit = false;
    }

    
    void Update()
    {
        RaycastHit hit;
        // casts ray from camera in the forward direction and writes down to hit
        // the info of GameObject that was hit by the ray is written to hit
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
        {
            // not allow player to pick more than one weapon of a type
            if (isWeaponAlreadyPickedByPlayer(playerPistol, hit) || 
                isWeaponAlreadyPickedByPlayer(playerGrenade, hit))
            {
                return;
            }
                         
            // check if "this" is the GameObject that was hit
            if (hit.transform.gameObject.name == this.gameObject.name && hit.distance < 10)
            {
                if (!isTriggerHit)
                {
                    isTriggerHit = true;
                    fireCrosshair.SetActive(false);
                    actionCrosshair.SetActive(true);                   
                }

                if (Input.GetButtonDown("Action"))
                {
                    if (this.gameObject.tag == playerPistol.tag)
                    {             
                        playerPistol.SetActive(true);
                    }
                    else // its the grenade
                    {                 
                        playerGrenade.SetActive(true);
                    }

                    this.gameObject.SetActive(false);
                    fireCrosshair.SetActive(true);
                    actionCrosshair.SetActive(false);
                }
                
            }
            else // trigger not hit
            {
                if (isTriggerHit)
                {
                    isTriggerHit = false;              
                    fireCrosshair.SetActive(true);
                    actionCrosshair.SetActive(false);
                }
            }
        }

    }


    bool isWeaponAlreadyPickedByPlayer(GameObject weapon, RaycastHit hit)
    {
        return weapon.activeSelf && hit.transform.gameObject.tag == weapon.tag;        
    }
}
