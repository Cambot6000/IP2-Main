using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public float health = 100;
    public float speed = 100;
    public int damage = 5;
    public Vector3 target;
    public List<Vector3> pathWaypoint = new List<Vector3>();
    public int waypointNumber;

   
    private GameUI gameUI;
    // All this is to handles slows
    private float originalSpeed;
    private float slowTimer;
    private float slowDuration;
    private float slowMultiplier = 1f; // 1 = no slow

    [Header("Poison Stuff")]
    public bool isPoisoned = false;
    public int poisonTimes; //How many times the enemy will take poison damage before the effect wears off
    public int poisonDamage;
    private int counter;
    public int poisonWait; //How long it takes for the enemy to take damage from poison

    private void Start()
    {
        EnemiesSpawner grid = FindAnyObjectByType<EnemiesSpawner>();
        gameUI =FindAnyObjectByType<GameUI>();
        if (grid == null)
        {
            Debug.LogError("GridLayout not assigned!");
            enabled = false;
            return;
        }

        originalSpeed = speed;

        waypointNumber = 0;
        pathWaypoint = grid.pathWaypoint;
        if (pathWaypoint == null || pathWaypoint.Count == 0)
        {
            Debug.LogError("No waypoints assigned!");
            enabled = false;
            return;
        }

        target = pathWaypoint[waypointNumber];

        counter = 0;


       
    }


    private void Update()
    {
        if (slowMultiplier != 1f) // Checks if slow is applied
        {
            slowTimer += Time.deltaTime;
            if (slowTimer >= slowDuration) // If slow over, return to normal speed
            {
                slowMultiplier = 1f;
                slowTimer = 0f;
                slowDuration = 0f;
                speed = originalSpeed;
            }
            else
            {
                // Keep current slowed speed
                speed = originalSpeed * slowMultiplier;
            }
        }
        else
        {
            // Leave normal speed
            speed = originalSpeed;
        }
        
        
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        Vector3 direction = (target - transform.position).normalized;
        quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
        transform.rotation,
        targetRotation,
        100 * Time.deltaTime);


        if ((Vector3)transform.position == target)
        {
            waypointNumber++;
            ChangeWaypoint();
        }
   

        if (health <= 0)
        {
            Destroy(gameObject);
            if (MoneyManager.instance != null)
            {
             MoneyManager.instance.AddGold(50);
            }
        }


        if (waypointNumber == pathWaypoint.Count - 1)
        {
            gameUI.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damageAmount)
    { 
        health -= damageAmount;
    }


    public void ChangeWaypoint()
    {
        target = pathWaypoint[waypointNumber];
        //print($"{pathWaypoint[waypointNumber]}");
    }

    // ApplySlow expects slowAmount as a multiplier (e.g. 0.5f to halve speed)
    // and duration in seconds. Calling again refreshes the slow with the new values.


    public void ApplySlow(float slowAmount, float duration)
    {

        slowMultiplier = slowAmount;
        slowDuration = duration;
        slowTimer = 0f;
        speed = originalSpeed * slowMultiplier;         // Apply slow
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Poison")
        {
            if (!isPoisoned)
            {
                poisonTimes = collision.gameObject.GetComponent<PoisonousScript>().poisonTimes;
                poisonDamage = collision.gameObject.GetComponent<PoisonousScript>().poisonDamage;
                poisonWait = collision.gameObject.GetComponent<PoisonousScript>().poisonWait;
                StartCoroutine(PoisonTick());
                isPoisoned = true;
            }
        }
    }

    private IEnumerator PoisonTick()
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
        else if (counter < poisonTimes)
        {
            isPoisoned = true;
            StartCoroutine(PoisonTick());
        }
    }
}



