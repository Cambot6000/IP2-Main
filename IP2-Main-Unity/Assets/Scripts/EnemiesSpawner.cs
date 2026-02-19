using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public enum Difficulty { Easy, Meduim, Hard }
    [Header("Type of Enemies")]
    public Difficulty chosenDifficulty = Difficulty.Easy;
    public int numberOfEnemies;
    public int hopperAmount;
    public int hopperCounter;
    public int segwayAmount;
    public int segwayCounter;
    public int flyingSauserAmount;
    public int flyingSauserCounter;
    public bool moreEnemies;
    public bool upgradedEnemies;




    [Header("Waves")]
    public int waveNumber;
    public float waveTimmerMax;
    public float enemySpawnTimerMax;

    public float waveTimmer;
    public float enemySpawnTimer;


    public List<Vector3> pathWaypoint = new List<Vector3>();
    public GameObject[] enemies;

    private void Start()
    {
        waveNumber = 1;
        EnemiesAmount();

    }

    private void Update()
    {
        enemySpawnTimer = enemySpawnTimer + Time.deltaTime;

        waveTimmer = waveTimmer + Time.deltaTime;


        if (enemySpawnTimer > enemySpawnTimerMax)
        {
            if (hopperCounter < hopperAmount)
            {
                Instantiate(enemies[0], transform.position, transform.rotation);
                hopperCounter++;
            }
            else if (segwayCounter < segwayAmount)
            {
                Instantiate(enemies[1], transform.position, transform.rotation);
                segwayCounter++;
            }
            else if (flyingSauserCounter < flyingSauserAmount)
            {
                Instantiate(enemies[2], transform.position, transform.rotation);
                flyingSauserCounter++;
            }
            enemySpawnTimer = 0;
        }

        if (waveTimmer > waveTimmerMax)
        {
            waveNumber++;
            if (enemySpawnTimerMax == 1f)
            {
                moreEnemies = false;

                upgradedEnemies = true;
            }
            hopperCounter = 0;
            segwayCounter = 0;
            flyingSauserCounter = 0;
            if (moreEnemies)
            {
                enemySpawnTimerMax--;
                EnemiesAmount();
            }
            else if (upgradedEnemies)
            {
                UpgradedEnemies();
            }

            waveTimmer = 0;
        }


    }
    public void EnemiesAmount()
    {

        numberOfEnemies = Mathf.RoundToInt(waveTimmerMax / enemySpawnTimerMax);

        if (chosenDifficulty == Difficulty.Easy)
        {
            hopperAmount = Mathf.RoundToInt(numberOfEnemies * 0.5f);

            segwayAmount = Mathf.RoundToInt(numberOfEnemies * 0.3f);

            flyingSauserAmount = Mathf.RoundToInt(numberOfEnemies * 0.2f);

        }
        else if (chosenDifficulty == Difficulty.Meduim)
        {
            hopperAmount = Mathf.RoundToInt(numberOfEnemies * 0.35f);

            segwayAmount = Mathf.RoundToInt(numberOfEnemies * 0.4f);

            flyingSauserAmount = Mathf.RoundToInt(numberOfEnemies * 0.25f);

        }
        else if (chosenDifficulty == Difficulty.Hard)
        {
            hopperAmount = Mathf.RoundToInt(numberOfEnemies * 0.15f);

            segwayAmount = Mathf.RoundToInt(numberOfEnemies * 0.35f);

            flyingSauserAmount = Mathf.RoundToInt(numberOfEnemies * 0.5f);

        }
    }
    public void UpgradedEnemies()
    {

        if (hopperAmount >= Mathf.RoundToInt(numberOfEnemies * 0.2f))
        {
            hopperAmount = hopperAmount - 2;
            segwayAmount += 2;
        }
        else if (segwayAmount >= Mathf.RoundToInt(numberOfEnemies * 0.2f))
        {
            segwayAmount = hopperAmount - 2;
            flyingSauserAmount += 2;
        }






    }
}




