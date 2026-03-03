using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public Enemies target;
    public float lifeTime = 5f;
    public bool slowingShot = false;
    private float timer;

    [Header("Poison Stuff")]
    public GameObject poisonPool; //The poison thing that spawns on the floor after the projectile hits
    public bool poison;

    private void Update()
    {
        
        timer += Time.deltaTime;
        if (timer >= lifeTime) // Destroy after lifetime to prevent infinite of them
        {
            Destroy(gameObject);
            return;
        }

        if (target != null)
        {
            if (target.health <= 0) // Destroy when the target is dead
            {
                Destroy(gameObject);
                return;
            }

            // Move toward target
            Vector3 targetPos = target.transform.position;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            // Deal damage and destroy (happens when very very close)
            if ((transform.position - targetPos).sqrMagnitude <= (step * step) + 0.001f)
            {
                if (slowingShot)
                {
                    target.ApplySlow(0.5f, 5f); // Applies a slow (50% speed for 5 seconds)
                }

                if (poison && !slowingShot)
                {
                    Instantiate(poisonPool, new Vector3
                        (target.pathWaypoint[target.waypointNumber].x,
                        target.pathWaypoint[target.waypointNumber].y, 
                        target.pathWaypoint[target.waypointNumber].z), 
                        Quaternion.identity);
                }

                target.health -= damage;
                Destroy(gameObject);
            }
        }

        else
        {
            // If the target dies, just keep going forward until lifetime runs out
            // (Not sure this is needed but hey)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}