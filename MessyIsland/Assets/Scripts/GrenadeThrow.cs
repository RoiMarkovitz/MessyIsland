using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    const float THROW_AFTER_DELAY = 3.0f;
    const float THROW_BEFORE_DELAY = 1.2f;
    const float THROW_SPEED = 27.5f;
    const float DAMAGE = 70.0f;
    

    [SerializeField] GameObject player;  
    AudioSource explosionSound;
    GameObject grenade;
    [SerializeField] GameObject pistol;
    Player playerScript;
    PlayerMotion playerMotionScript;
    NPC npcScript;

    bool isThrowingAllowed;

    void Start()
    {
        isThrowingAllowed = true;
        grenade = this.transform.GetChild(0).gameObject;
        explosionSound = GetComponent<AudioSource>();
        playerScript = player.GetComponent<Player>();
        playerMotionScript = player.GetComponent<PlayerMotion>(); 

    }

    
    void Update()
    {
          
    }

    void FixedUpdate()
    {
        if (Input.GetKey("q") && isThrowingAllowed && playerScript.getHasGrenade())
        {
            StartCoroutine(throwGrenade());
        }
    }

    IEnumerator throwGrenade()
    {
        isThrowingAllowed = false;

        bool hasPistol = playerScript.getHasPistol();
        if (hasPistol)
        {
            pistol.SetActive(false);
        }
    
        playerMotionScript.setIsThrowingGrenade(true);      
        Animator animator = player.GetComponent<Animator>();
        animator.SetInteger("status", (int)PlayerMotion.PlayerAnimStatus.GrenadeThrow);
   
        Vector3 velocity = transform.forward * THROW_SPEED;
        
        yield return new WaitForSeconds(THROW_BEFORE_DELAY);

        if (hasPistol)
        {
            pistol.SetActive(true);
        }
      
        playerMotionScript.setIsThrowingGrenade(false);

        GameObject clonedGrenade = Instantiate(grenade, grenade.transform.position, grenade.transform.rotation);
        clonedGrenade.SetActive(true);

        Rigidbody rbClonedGrenade = clonedGrenade.GetComponent<Rigidbody>();
        rbClonedGrenade.useGravity = true;
        rbClonedGrenade.AddForce(velocity, ForceMode.Impulse);
        
        yield return new WaitForSeconds(THROW_AFTER_DELAY);

        explosionSound.Play();

        hideClonedGrenadeMeshParts(clonedGrenade);
            
        GameObject explosion = clonedGrenade.transform.GetChild(4).gameObject;
        explosion.SetActive(true);

        Collider[] objectsCollider = Physics.OverlapSphere(explosion.transform.position, 10.0f);

        for (int i = 0; i < objectsCollider.Length; i++)
        {
            if (objectsCollider[i] != null)
            {
                if (objectsCollider[i].tag == "Ninja" && this.tag == "SwatGrenade")
                {
                    npcScript = objectsCollider[i].gameObject.GetComponent<NPC>();      
                    npcScript.takeDamage(DAMAGE, playerScript.getNickname());
                                                     
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

    void hideClonedGrenadeMeshParts(GameObject clonedGrenade)
    {
        clonedGrenade.transform.GetChild(0).gameObject.SetActive(false);
        clonedGrenade.transform.GetChild(1).gameObject.SetActive(false);
        clonedGrenade.transform.GetChild(2).gameObject.SetActive(false);
        clonedGrenade.transform.GetChild(3).gameObject.SetActive(false);
    }

}
