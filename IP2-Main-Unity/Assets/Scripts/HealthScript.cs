using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    private bool isDead = false;

    [Header("Poison Stuff")]
    public bool isPoisoned = false;
    public int poisonTimes; //How many times the enemy will take poison damage before the effect wears off
    public int poisonDamage;
    private int counter;
    public int poisonWait; //How long it takes for the enemy to take damage from poison

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Poison")
        {
            if (!isPoisoned)
            {
                poisonTimes = other.gameObject.GetComponent<PoisonousScript>().poisonTimes;
                poisonDamage = other.gameObject.GetComponent<PoisonousScript>().poisonDamage;
                poisonWait = other.gameObject.GetComponent<PoisonousScript>().poisonWait;
                StartCoroutine(PoisonTick());
                isPoisoned = true;
            }
        }
    }

    private IEnumerator PoisonTick() //45 mins so far
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
