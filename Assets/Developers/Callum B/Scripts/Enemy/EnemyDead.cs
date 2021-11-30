using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    private Animator animator;
    public float health = 50.0f;
    public int zone1enemiescount = 7;
    
   // private bool isDead = false;
    public GameObject ragdoll;
    private EnemyKillingObjective enemykillingobjective;
    private Objective objectivescript;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemykillingobjective = GetComponent<EnemyKillingObjective>();
        objectivescript = GetComponent<Objective>();
    }

    public void Update()
    {
        /*if (isDead)
        {
            animator.SetBool("Dead", true);
        }
        else
        {
            animator.SetBool("Dead", false);
        }*/

        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
            zone1enemiescount--;
        }
    }

    private void Die()
    {
        OnDeath();
        // Destroy(gameObject);
    }

    public void OnDeath()
    {
        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(gameObject);                
        Debug.Log("1 enemy dead");

        if (zone1enemiescount <= 0)
        {
            Debug.Log("enemies are all dead");
            objectivescript.iscompleted = true;
        }
    }
}
