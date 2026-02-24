using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Poison Stuff")]
    public bool isPoisoned = false;
    public int poisonTimes;
    public int poisonDamage;
    private int counter;
    public int poisonWait;

    void Start()
    {
        currentHealth = maxHealth;
        counter = 0;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Poison")
        {
            if (!isPoisoned)
            {
                StartCoroutine(PoisonTick());
                isPoisoned = true;
            }
        }
    }

    private IEnumerator PoisonTick() //25 mins so far
    {
        yield return new WaitForSeconds(poisonWait);
        TakeDamage(poisonDamage);
        counter += 1;
        PoisonValidate();
    }

    void PoisonValidate()
    {
        if (counter >= poisonTimes)
        {
            isPoisoned = false;
        }
        else if(counter < poisonTimes)
        {
            isPoisoned = true;
            StartCoroutine(PoisonTick());
        }
    }
}
