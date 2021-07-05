using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] GameObject target;

    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation = target.transform.rotation;      
    }
}
