using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    const int HEALTH = 100;

    [SerializeField] string nickname;
    protected int health;
    protected bool hasPistol;
    protected bool hasGrenade;

    void Start()
    {
        health = HEALTH;
        hasPistol = false;
        hasGrenade = false;
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
        health -= reduction;
    }

    public void setNickname(string newNickname)
    {
        nickname = newNickname;
    }

    public string getNickname()
    {
        return nickname;
    }
}
