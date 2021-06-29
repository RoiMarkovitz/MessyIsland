using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Humanoid : MonoBehaviour
{
    const int MAX_HEALTH = 100;
    const int MIN_HEALTH = 0;

    [SerializeField] string nickname;
    protected int health;
    protected bool hasPistol;
    protected bool hasGrenade;
    protected bool isAlive;

    public Humanoid()
    {
        health = MAX_HEALTH;
        hasPistol = false;
        hasGrenade = false;
        isAlive = true;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void setHasPistol(bool value)
    {
        hasPistol = value;
    }

    public bool getHasPistol()
    {
        return hasPistol;
    }

    public void setHasGrenade(bool value)
    {
        hasGrenade = value;
    }

    public bool getHasGrenade()
    {
        return hasGrenade;
    }

    public int getHealth()
    {
        return health;
    }

    public void reduceHealth(int reduction)
    {
        if (MAX_HEALTH >= health && health > MIN_HEALTH)
        { 
            health -= reduction;

            if (health < 0)
            {
                health = MIN_HEALTH;
            }
        }

    }

    public void setNickname(string newNickname)
    {
        nickname = newNickname;
    }

    public string getNickname()
    {
        return nickname;
    }

    public bool getIsAlive()
    {
        if (health <= 0)
        {
            isAlive = false;
        }

        return isAlive;
    }
}
