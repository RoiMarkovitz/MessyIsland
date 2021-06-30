using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    public const float DAMAGE = 20.0f;

    string owner;
    
    void Start()
    {
        
    }
 
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    public string getOwner()
    {
        return owner;
    }

    public void setOwner(string theOwner)
    {
        this.owner = theOwner;
    }
}
