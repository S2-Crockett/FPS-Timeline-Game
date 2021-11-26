using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Image barImage;
    private Image damagedBarImage;
    private float damageHealthTimerMax = 1f;
    
    [Header("Damage")]
    public Color damagedColor;
    public float damageHealthTimer;
    public float shrinkSpeed = 3f;
    
    private void Awake()
    {
        barImage = transform.Find("HealthAmount").GetComponent<Image>();
        damagedBarImage = transform.Find("HealthChangeAmount").GetComponent<Image>();
        damagedBarImage.color = damagedColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        damageHealthTimer -= Time.deltaTime;
        if (damageHealthTimer < 0)
        {
            if (barImage.fillAmount < damagedBarImage.fillAmount)
            {
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }
    }

    public void OnHealed()
    {
        SetHealth(UIManager.Instance.GetHealthNormalized());
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    public void OnDamage()
    {
        damageHealthTimer = damageHealthTimerMax;
        SetHealth(UIManager.Instance.GetHealthNormalized());
    }

    private void SetHealth(float health)
    {
        barImage.fillAmount = health;
    }
}
