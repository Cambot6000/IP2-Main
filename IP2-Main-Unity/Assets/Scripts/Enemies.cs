using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public float health = 0;
    public float speed = 100;
    public int damage = 5;
    public Vector3 target;
    public List<Vector3> pathWaypoint = new List<Vector3>();
    public int waypointNumber;

    private GameUI gameUI;
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
        waypointNumber = 0;
        pathWaypoint = grid.pathWaypoint;
        if (pathWaypoint == null || pathWaypoint.Count == 0)
        {
            Debug.LogError("No waypoints assigned!");
            enabled = false;
            return;
        }

        target = pathWaypoint[waypointNumber];
        

        
    }


    private void Update()
    {

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
   

        if (waypointNumber == pathWaypoint.Count - 1)
        {
            gameUI.TakeDamage(damage);
            Destroy(gameObject);
        }
    }


    public void ChangeWaypoint()
    {

        target = pathWaypoint[waypointNumber];
        //print($"{pathWaypoint[waypointNumber]}");


    }

}



