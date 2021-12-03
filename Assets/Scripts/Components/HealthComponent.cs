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

    private PlayerDeathController _deathController;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        _deathController = GetComponent<PlayerDeathController>();
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
                _deathController.SetIsDead(true);
            }
            UIManager.Instance.UpdateHealthDamage();
        }
        else
        {
            // if our damage is going to destroy our shield then we need to work
            // out the different and take that from our health
            if (damageAmount >= shield)
            {
                int damageDif = damageAmount - shield;
                shield = 0;
                health -= damageDif;
                if (damageDif >= health)
                {
                    health = 0;
                    _deathController.SetIsDead(true);
                }
                UIManager.Instance.UpdateHealthDamage();
            }
            else
            {
                // if its not more than our current shield just take it away from our shield
                shield -= damageAmount;
                if (shield < 0)
                {
                    shield= 0;
                }
                UIManager.Instance.UpdateHealthDamage();
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
        UIManager.Instance.UpdateHealthHeal();
    }

    public void AddShield(int amount)
    {
        shield += amount;
        if (shield > maxShield)
        {
            shield = maxShield;
        }
        UIManager.Instance.UpdateHealthHeal();
    }

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }
    
    public float GetShieldNormalized()
    {
        return (float)shield / maxShield;
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
