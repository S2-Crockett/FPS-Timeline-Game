using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemy;
    
    [Header("Information")]
    public bool dead;
    public float health = 200.0f;

    private LineOfSight weaponScript;

    private void Start()
    {
        weaponScript = GetComponent<LineOfSight>();
    }

    // different way // headshot damage?
    public void TakeDamage(float damage)
    {
        weaponScript.stages = LineOfSight.Stages.FOUND;
        weaponScript.SendFoundAlert();
        health -= damage;
        if (health <= 0)
        {
            dead = true;
        }
    }
}
