using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
    public float health = 50.0f;

    public bool dead = false;

    private void Start()
    {
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            dead = true;
        }
    }
}
