using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    const float THROW_AFTER_DELAY = 3.0f;
    const float THROW_BEFORE_DELAY = 1.2f;
    const float THROW_SPEED = 20.0f;
    const float DAMAGE = 80.0f;

    bool isThrowingAllowed;
    bool isNPC;

    [SerializeField] GameObject user;  
    
    GameObject grenade;
    [SerializeField] GameObject pistol;
    Humanoid userScript;  
    NPC npcScript;
    
    void Start()
    {       
        isThrowingAllowed = true;
        grenade = this.transform.GetChild(0).gameObject;
        
        userScript = user.GetComponent<Humanoid>();
        isNPC = userScript.getIsNPC();

        if (isNPC)
        {
            npcScript = user.GetComponent<NPC>();
        }
                     
    }

    
    void Update()
    {
          
    }

    void FixedUpdate()
    {
        if (isThrowingAllowed && userScript.getHasGrenade())
        {
            if (isNPC)
            {
                if (npcScript.getIsThrowingGrenade())
                {
                    StartCoroutine(throwGrenade());
                }
            } else if(Input.GetKey("q"))
            {
                StartCoroutine(throwGrenade());
            }
        }          
    }

    IEnumerator throwGrenade()
    {
        isThrowingAllowed = false;

        bool hasPistol = userScript.getHasPistol();
        if (hasPistol)
        {
            pistol.SetActive(false);
        }

        userScript.setIsThrowingGrenadeAnim(true);      
        Animator animator = user.GetComponent<Animator>();
        if (isNPC)
        {
            animator.SetInteger("status", (int)NPC.NPCAnimStatus.GrenadeThrow);
        }
        else
        {
            animator.SetInteger("status", (int)Player.PlayerAnimStatus.GrenadeThrow);
        }
        
   
        Vector3 velocity = transform.forward * THROW_SPEED;
        
        yield return new WaitForSeconds(THROW_BEFORE_DELAY);

        if (hasPistol)
        {
            pistol.SetActive(true);
        }

        userScript.setIsThrowingGrenadeAnim(false);

        GameObject clonedGrenade = Instantiate(grenade, grenade.transform.position, grenade.transform.rotation);
        clonedGrenade.SetActive(true);

        Rigidbody rbClonedGrenade = clonedGrenade.GetComponent<Rigidbody>();
        rbClonedGrenade.useGravity = true;
       
        rbClonedGrenade.AddForce(velocity, ForceMode.Impulse);
        
        yield return new WaitForSeconds(THROW_AFTER_DELAY);

        AudioSource explosionSound = rbClonedGrenade.GetComponent<AudioSource>();
        explosionSound.Play();

        hideClonedGrenadeMeshParts(clonedGrenade);
            
        GameObject explosion = clonedGrenade.transform.GetChild(4).gameObject;
        explosion.SetActive(true);

        Collider[] objectsCollider = Physics.OverlapSphere(explosion.transform.position, 10.0f);

        for (int i = 0; i < objectsCollider.Length; i++)
        {
            if (objectsCollider[i] != null)
            {
                if ((objectsCollider[i].tag == "Ninja" && this.tag == "SwatGrenade")
                    || objectsCollider[i].tag == "Swat" && this.tag == "NinjaGrenade")
                {
                    damageEnemy(objectsCollider[i]);                
                }
                                     
                //Rigidbody rbo = objectsCollider[i].GetComponent<Rigidbody>();
                //if (rbo!= null)
                //{ 
                //rbo.AddExplosionForce(1500.0f, explosion.transform.position, 10.0f);
                //}
            }
        }
      
        isThrowingAllowed = true;
        
        yield return new WaitForSeconds(2.0f);
        Destroy(clonedGrenade);
        
    }

    void damageEnemy(Collider enemy)
    {
        Humanoid enemyScript = enemy.gameObject.GetComponent<Humanoid>();
        if (!enemyScript.getIsNPC())
        {
            Player playerScript = enemy.gameObject.GetComponent<Player>();
            playerScript.takeDamage(DAMAGE, userScript.getNickname());        
        }
        else
        {
            NPC npcScript = enemy.gameObject.GetComponent<NPC>();
            npcScript.takeDamage(DAMAGE, userScript.getNickname());
        }       
    }

    void hideClonedGrenadeMeshParts(GameObject clonedGrenade)
    {
        clonedGrenade.transform.GetChild(0).gameObject.SetActive(false);
        clonedGrenade.transform.GetChild(1).gameObject.SetActive(false);
        clonedGrenade.transform.GetChild(2).gameObject.SetActive(false);
        clonedGrenade.transform.GetChild(3).gameObject.SetActive(false);
    }

}
