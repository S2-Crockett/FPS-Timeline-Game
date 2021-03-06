using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HealthComponent : MonoBehaviour
{
    [Header("Health")]
    public int health;
    public int maxHealth;

    [Header("Shield")] 
    public int shield = 50;
    public int maxShield = 50;

    private PlayerDeathController _deathController;
    private Volume postProcessing;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        _deathController = GetComponent<PlayerDeathController>();
        postProcessing = _deathController.postProcessing;
        UIManager.Instance.SetHealthShield(maxHealth, maxShield);
    }

    public void Damage(int damageAmount)
    {
        //if our shield is at 0 then take it out of our health
        if (shield <= 0)
        {
            health -= damageAmount;
            if (health <= 0)
            {
                health = 0;
                _deathController.SetIsDead(true);
                UIManager.Instance.healthPanel.ClearPoints();
            }
            UIManager.Instance.UpdateHealth(health, shield);
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
                    UIManager.Instance.healthPanel.ClearPoints();
                }
                UIManager.Instance.UpdateHealth(health, shield);
            }
            else
            {
                // if its not more than our current shield just take it away from our shield
                shield -= damageAmount;
                if (shield < 0)
                {
                    shield= 0;
                }
                UIManager.Instance.UpdateHealth(health, shield);
            }
        }
        
        if (postProcessing.profile.TryGet<Vignette>(out var vignette))
        {
            vignette.intensity.value = 0.2f;
            LeanTween.value(gameObject, vignette.intensity.value, 0.35f, 0.05f)
                .setOnUpdate((value) =>
                {
                    vignette.intensity.value = value;
                })
                .setOnComplete(ResetDamageIndicator);
        }
    }

    private void ResetDamageIndicator()
    {
        if (postProcessing.profile.TryGet<Vignette>(out var vignette))
        {
            LeanTween.value(gameObject, vignette.intensity.value, 0.2f, 0.25f)
                .setOnUpdate((value) =>
                {
                    vignette.intensity.value = value;
                });
        }
    }

    public void AddHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UIManager.Instance.UpdateHealth(health, shield);
    }

    public void AddShield(int amount)
    {
        shield += amount;
        if (shield > maxShield)
        {
            shield = maxShield;
        }
        UIManager.Instance.UpdateHealth(health, shield);
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
