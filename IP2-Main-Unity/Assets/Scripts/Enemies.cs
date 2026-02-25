using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public int health = 0;
    public float speed = 100;
    public Vector3 target;
    public List<Vector3> pathWaypoint = new List<Vector3>();
    public int waypointNumber;

    // All this is to handles slows
    private float originalSpeed;
    private float slowTimer;
    private float slowDuration;
    private float slowMultiplier = 1f; // 1 = no slow

    private void Start()
    {
        EnemiesSpawner grid = FindAnyObjectByType<EnemiesSpawner>();
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
        for (int i = 0; i < pathWaypoint.Count; i++)
        {
            pathWaypoint[i] = new Vector3(
                 pathWaypoint[i].x,
                 transform.position.y,
                 pathWaypoint[i].z
                );
        }
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
            Destroy(gameObject);
        }
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
}


