using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Humanoid : MonoBehaviour
{
    const float MAX_HEALTH = 100.0f;
    const float MIN_HEALTH = 0.0f;

    [SerializeField] string nickname;
    protected float startHealth;
    protected float currentHealth;
    protected bool hasPistol;
    protected bool hasGrenade;
    protected bool isAlive;
    protected bool isNPC;
    protected bool isThrowingGrenadeAnim;

    [SerializeField] protected GameObject pistol;
    [SerializeField] Image healthBar;

    [SerializeField] protected GameObject round;
    protected RoundManager roundManagerScript;

    protected Animator animator;

    void Awake()
    {
        startHealth = MAX_HEALTH;
        currentHealth = startHealth;
        hasPistol = false;
        hasGrenade = false;
        isAlive = true;
        animator = GetComponent<Animator>();
        roundManagerScript = round.GetComponent<RoundManager>();
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

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    virtual public void takeDamage(float reduction, string enemyNickname)
    {
        if (MAX_HEALTH >= currentHealth && currentHealth > MIN_HEALTH)
        {
            currentHealth -= reduction;
            healthBar.fillAmount = currentHealth / startHealth;

            if (currentHealth <= 0)
            {
                currentHealth = MIN_HEALTH;

                GameManager.instance.showKillText(enemyNickname, this.nickname);
                healthBar.GetComponentInParent<Image>().GetComponentInParent<Canvas>().gameObject.SetActive(false);
                this.gameObject.tag = "Dead";
                isAlive = false;

                if (hasPistol)
                {
                    pistol.SetActive(false);
                }
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
        if (currentHealth <= 0)
        {
            isAlive = false;
        }

        return isAlive;
    }

    public bool getIsNPC()
    {
        return isNPC;
    }

    public void setIsThrowingGrenadeAnim(bool value)
    {
        isThrowingGrenadeAnim = value;
    }
}
