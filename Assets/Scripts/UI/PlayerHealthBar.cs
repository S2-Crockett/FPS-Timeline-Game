using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Image healthBarImage;
    private Image healthDamagedBarImage;
    private Image shieldBarImage;
    private Image shieldDamagedBarImage;
    private float damageHealthTimerMax = 1f;
    
    [Header("Damage")]
    public Color healthDamagedColor;
    public Color shieldDamagedColor;
    public float damageHealthTimer;
    public float shrinkSpeed = 3f;
    
    private void Awake()
    {
        healthBarImage = transform.Find("HealthAmount").GetComponent<Image>();
        healthDamagedBarImage = transform.Find("HealthChangeAmount").GetComponent<Image>();
        shieldBarImage = transform.Find("ShieldAmount").GetComponent<Image>();
        shieldDamagedBarImage = transform.Find("ShieldChangeAmount").GetComponent<Image>();
        
        healthDamagedBarImage.color = healthDamagedColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        healthDamagedBarImage.fillAmount = healthBarImage.fillAmount;
        shieldDamagedBarImage.fillAmount = healthBarImage.fillAmount;
        SetHealth(UIManager.Instance.GetHealthNormalized());
        SetShield(UIManager.Instance.GetShieldNormalized());
    }

    // Update is called once per frame
    void Update()
    {
        damageHealthTimer -= Time.deltaTime;
        if (damageHealthTimer < 0)
        {
            if (shieldBarImage.fillAmount < shieldDamagedBarImage.fillAmount)
            {
                shieldDamagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
            
            if (healthBarImage.fillAmount < healthDamagedBarImage.fillAmount)
            {
                healthDamagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
          
        }
    }

    public void OnHealed()
    {
        SetHealth(UIManager.Instance.GetHealthNormalized());
        SetShield(UIManager.Instance.GetShieldNormalized());
        healthDamagedBarImage.fillAmount = healthBarImage.fillAmount;
        shieldDamagedBarImage.fillAmount = shieldBarImage.fillAmount;
    }

    public void OnDamage()
    {
        damageHealthTimer = damageHealthTimerMax;
        SetHealth(UIManager.Instance.GetHealthNormalized());
        SetShield(UIManager.Instance.GetShieldNormalized());
    }

    private void SetHealth(float health)
    {
        healthBarImage.fillAmount = health;
    }

    private void SetShield(float shield)
    {
        shieldBarImage.fillAmount = shield;
    }
    
    
}
