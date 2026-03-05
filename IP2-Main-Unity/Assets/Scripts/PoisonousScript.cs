using System.Collections;
using UnityEngine;

public class PoisonousScript : MonoBehaviour
{
    public int poisonTimes; //How many times the enemy will take poison damage before the effect wears off
    public int poisonDamage; //How much damage the poison deals per poison "tick"
    public int poisonWait; //How long it takes for the enemy to take damage from poison
    [SerializeField] private bool pool;
    public float lifeSpan; //How long a poison pool lasts for before disappearing

    private void Start()
    {
        if (pool)
        {
            StartCoroutine(Lifetime());
        }
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }
}
