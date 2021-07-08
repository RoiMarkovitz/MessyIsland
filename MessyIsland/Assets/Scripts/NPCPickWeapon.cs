using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPickWeapon : MonoBehaviour
{
    GameObject weapon;
    bool isWeaponPicked;

    void Start()
    {
        weapon = transform.parent.GetComponentInParent<Transform>().gameObject;
    }


    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (!isWeaponPicked)
        {
            if (other.gameObject.tag == "Swat" || other.gameObject.tag == "Ninja")
            {

                Humanoid script = other.gameObject.GetComponent<Humanoid>();
                if (script.getIsNPC())
                {

                    NPC npcScript = other.gameObject.GetComponent<NPC>();
                    if (weapon.gameObject.tag == "Grenade" && !npcScript.getHasGrenade())
                    {        
                        isWeaponPicked = true;
                        weapon.SetActive(false);
                        npcScript.pickGrenade();

                    }
                    else if (weapon.gameObject.tag == "Pistol" && !npcScript.getHasPistol())
                    {
                        isWeaponPicked = true;
                        weapon.SetActive(false);
                        npcScript.pickPistol();
                    }
                }

            }
        }
    }
}
