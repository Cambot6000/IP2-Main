using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public int health = 0;
    public float speed = 100;
    public Vector3 target;
    public List<Vector3> pathWaypoint = new List<Vector3>();
    public int waypointNumber;


    private void Start()
    {
        BuildGrid grid = FindAnyObjectByType<BuildGrid>();
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

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if ((Vector3)transform.position == target)
        {
            waypointNumber++;
            ChangeWaypoint();
        }
    }


    public void ChangeWaypoint()
    {

        target = pathWaypoint[waypointNumber];
        print($"{waypointNumber}");


    }


}

