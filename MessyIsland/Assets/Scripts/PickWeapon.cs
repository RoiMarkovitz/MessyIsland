using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickWeapon : MonoBehaviour
{
    
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject playerPistol;
    [SerializeField] GameObject playerGrenade;
    [SerializeField] GameObject canvas;
    GameObject fireCrosshair;
    GameObject actionCrosshair;
    GameObject grenadePlaceholderImage;
    GameObject pistolPlaceholderImage;
    GameObject activePistolImage;
    GameObject activeGrenadeImage;
    
    bool isTriggerHit;

    void Start()
    {
        isTriggerHit = false;
        fireCrosshair = canvas.transform.GetChild(0).gameObject;
        actionCrosshair = canvas.transform.GetChild(1).gameObject;
        grenadePlaceholderImage = canvas.transform.GetChild(2).gameObject;
        pistolPlaceholderImage = canvas.transform.GetChild(3).gameObject;
        activePistolImage = canvas.transform.GetChild(4).gameObject;
        activeGrenadeImage = canvas.transform.GetChild(5).gameObject;
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
                        pistolPlaceholderImage.SetActive(false);
                        activePistolImage.SetActive(true);
                    }
                    else // its the grenade
                    {                 
                        playerGrenade.SetActive(true);
                        grenadePlaceholderImage.SetActive(false);
                        activeGrenadeImage.SetActive(true);
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
