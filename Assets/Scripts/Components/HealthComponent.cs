using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Health")]
    public int health;
    public int maxHealth;

    [Header("Shield")] 
    public int shield;
    public int maxShield;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void Damage(int damageAmount)
    {
        //if our shield is at 0 then take it out of our health
        if (shield <= 0)
        {
            health -= damageAmount;
            if (health < 0)
            {
                health = 0;
            }
            UIManager.Instance.UpdateHealthBarDamage();
        }
        else
        {
            // if our damage is going to destroy our shield then we need to work
            // out the different and take that from our health
            if (damageAmount >= shield)
            {
                int damageDif = shield - damageAmount;
                shield = 0;
                health -= damageDif;
            }
            else
            {
                // if its not more than our current shield just take it away from our shield
                shield -= damageAmount;
                if (shield < 0)
                {
                    shield= 0;
                }
            }
        }
    }

    public void AddHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UIManager.Instance.UpdateHealthBarHeal();
    }

    public void AddShield(int amount)
    {
        shield += amount;
        if (shield > maxShield)
        {
            shield = maxShield;
        }
    }

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetShield()
    {
        return shield;
    }
}
