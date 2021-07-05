using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.tag == "Grenade")
       {         
            Collider collider = transform.parent.GetComponentInParent<Collider>();
            Physics.IgnoreCollision(other, collider);
       }
    }
}
