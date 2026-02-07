using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public GameObject[] enemies;

    private void Update()
    {
        Instantiate(enemies[0], transform.position, Quaternion.identity);
    }



}

