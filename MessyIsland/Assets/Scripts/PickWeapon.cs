using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickWeapon : MonoBehaviour
{
    
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject playerPistol;
    [SerializeField] GameObject playerGrenade;
    GameObject gameCanvas;
    GameObject fireCrosshair;
    GameObject actionCrosshair;
    GameObject grenadePlaceholderImage;
    GameObject pistolPlaceholderImage;
    GameObject activePistolImage;
    GameObject activeGrenadeImage;

    AudioSource pickupSound;

    [SerializeField] GameObject player;
    Player playerScript;
    
    bool isTriggerHit;

    void Start()
    {

        gameCanvas = GameObject.Find("CanvasRound");

        isTriggerHit = false;
        fireCrosshair = gameCanvas.transform.GetChild(0).gameObject;
        actionCrosshair = gameCanvas.transform.GetChild(1).gameObject;
        grenadePlaceholderImage = gameCanvas.transform.GetChild(2).gameObject;
        pistolPlaceholderImage = gameCanvas.transform.GetChild(3).gameObject;
        activePistolImage = gameCanvas.transform.GetChild(4).gameObject;
        activeGrenadeImage = gameCanvas.transform.GetChild(5).gameObject;
        
        playerScript = player.GetComponent<Player>();
        pickupSound = GetComponent<AudioSource>();
        
    }

    
    void Update()
    {
        RaycastHit hit;
        // casts ray from camera in the forward direction and writes down to hit
        // the info of GameObject that was hit by the ray is written to hit
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
        {
            //not allow player to pick more than one weapon of a type
            if (isWeaponAlreadyPickedByPlayer(hit) ||
                isWeaponAlreadyPickedByPlayer(hit))
            {
                return;
            }

            // check if "this" is the GameObject that was hit
            if (hit.transform.gameObject.name == this.gameObject.name && hit.distance < 8)
            {
                if (!isTriggerHit)
                {
                    isTriggerHit = true;
                    fireCrosshair.SetActive(false);
                    actionCrosshair.SetActive(true);                   
                }

                if (Input.GetButtonDown("Action"))
                {
                    pickupSound.Play();

                    if (this.gameObject.tag == playerPistol.tag)
                    {
                        Invoke("pickPistol", 0.5f);                                            
                    }
                    else // its the grenade
                    {                       
                        Invoke("pickGrenade", 0.5f);                                           
                    }


                    Invoke("hideWeapon", 0.5f);                 
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

    bool isWeaponAlreadyPickedByPlayer(RaycastHit hit)
    {
        if (hit.transform.gameObject.tag == "Pistol" && playerScript.getHasPistol())
        {
            return true;
        }

        if (hit.transform.gameObject.tag == "Grenade" && playerScript.getHasGrenade())
        {
            return true;
        }

        return false;
        
    }

    void pickPistol()
    {
        playerPistol.SetActive(true);
        pistolPlaceholderImage.SetActive(false);
        activePistolImage.SetActive(true);
        playerScript.setHasPistol(true);    
    }

    void pickGrenade()
    {
        playerScript.setHasGrenade(true);
        grenadePlaceholderImage.SetActive(false);
        activeGrenadeImage.SetActive(true);
        
    }

    void hideWeapon()
    {
        fireCrosshair.SetActive(true);
        actionCrosshair.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
