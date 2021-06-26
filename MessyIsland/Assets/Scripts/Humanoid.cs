using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    const int HEALTH = 100;

    [SerializeField] string nickname;
    protected int health;

    void Start()
    {
        health = HEALTH;
    }

    void Update()
    {
        
    }
}
