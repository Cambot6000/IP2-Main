using System.Collections;
using UnityEngine;

public class PoisonousScript : MonoBehaviour
{
    public int poisonTimes;
    public int poisonDamage;
    public int poisonWait;
    [SerializeField] private bool pool;
    public float lifeSpan;

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
