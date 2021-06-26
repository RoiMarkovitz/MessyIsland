using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShot : MonoBehaviour
{
    const float SHOOT_DELAY = 0.5f;
    const float BULLET_SPEED = 500.0f;

    GameObject bullet;
    GameObject muzzle;
    AudioSource bulletSound;
    
    bool isShootingAllowed;
    
    
    void Start()
    {
        bulletSound = GetComponent<AudioSource>();
        bullet = this.transform.GetChild(2).gameObject;
        muzzle = this.transform.GetChild(3).gameObject;
        isShootingAllowed = true;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && this.gameObject.activeSelf && isShootingAllowed)
        {       
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        isShootingAllowed = false;
        bulletSound.Play();
        GameObject clonedBullet = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation);
        clonedBullet.SetActive(true);

        Rigidbody rbClonedBullet = clonedBullet.GetComponent<Rigidbody>();
        rbClonedBullet.AddForce((muzzle.transform.forward * -1) * BULLET_SPEED, ForceMode.Impulse);
        
        yield return new WaitForSeconds(SHOOT_DELAY);

        isShootingAllowed = true;
        
    }

}
