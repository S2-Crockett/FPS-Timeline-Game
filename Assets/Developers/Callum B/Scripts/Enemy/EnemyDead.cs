using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
   private Animator animator;
    public float health = 50.0f;
   // private bool isDead = false;
    public GameObject ragdoll;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
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
    }
}
