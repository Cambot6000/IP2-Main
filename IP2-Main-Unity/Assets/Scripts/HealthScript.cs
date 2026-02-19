using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
