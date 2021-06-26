using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    const float THROW_AFTER_DELAY = 3.0f;
    const float THROW_BEFORE_DELAY = 1.2f;
    const float THROW_SPEED = 27.5f;
    

    [SerializeField] GameObject player;
    AudioSource explosionSound;
    GameObject grenade;
    
    
    bool isThrowingAllowed;

    void Start()
    {
        isThrowingAllowed = true;
        grenade = this.transform.GetChild(0).gameObject;
        explosionSound = GetComponent<AudioSource>();



    }

    
    void Update()
    {
        // need to add here that player has grenade check
        if (Input.GetKey("q") && isThrowingAllowed)
        {
            StartCoroutine(throwGrenade());
        }
    }

    IEnumerator throwGrenade()
    {
        isThrowingAllowed = false;
        
        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("isThrown", true);
        
        yield return new WaitForSeconds(THROW_BEFORE_DELAY);
        animator.SetBool("isThrown", false);
        GameObject clonedGrenade = Instantiate(grenade, grenade.transform.position, grenade.transform.rotation);
        clonedGrenade.SetActive(true);

        Rigidbody rbClonedGrenade = clonedGrenade.GetComponent<Rigidbody>();
        rbClonedGrenade.useGravity = true;
        rbClonedGrenade.AddForce(clonedGrenade.transform.forward * THROW_SPEED, ForceMode.Impulse);
        
        yield return new WaitForSeconds(THROW_AFTER_DELAY);

        explosionSound.Play();

        hideClonedGrenadeMeshParts(clonedGrenade);
            
        GameObject explosion = clonedGrenade.transform.GetChild(4).gameObject;
        explosion.SetActive(true);
        
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
