using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    
    [Header("Type of Enemies")]
    public GameSettings.Difficulty chosenDifficulty = GameSettings.chosenDifficulty;
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
    public float waitTimerMax;

    [Tooltip("Delay between rounds")]
    public float waitTimer;
    public float waveTimmer;
    public float enemySpawnTimer;

    public bool InWave;


    

    public List<Vector3> pathWaypoint = new List<Vector3>();
    public GameObject[] enemies;

    private void Start()
    {
        InWave = true;
        waveNumber = 1;
        EnemiesAmount();
        for (int i = 0; i < pathWaypoint.Count; i++)
        {
            pathWaypoint[i] = new Vector3(
                 pathWaypoint[i].x,
                 transform.position.y,
                 pathWaypoint[i].z
                );
        }

        chosenDifficulty = GameSettings.chosenDifficulty;
    }

    private void Update()
    {
        enemySpawnTimer = enemySpawnTimer + Time.deltaTime;

        waveTimmer = waveTimmer + Time.deltaTime;

        if(!InWave)
        waitTimer = waitTimer + Time.deltaTime;
       

        
        SpawnEnemies();
        EndOfWave();
    }

    public void EndOfWave()
    {
        if (waveTimmer > waveTimmerMax && InWave)
        {
            InWave = false;

            if (enemySpawnTimerMax == 5f)
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
            
        }
        if (waitTimer > waitTimerMax)
        {


                waveNumber++;
                InWave = true;
                waveTimmer = 0;
                waitTimer = 0;


        }
    }



    public void SpawnEnemies() 
    {
        if (enemySpawnTimer > enemySpawnTimerMax && InWave )
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

    }
    public void EnemiesAmount()
    {

        numberOfEnemies = Mathf.RoundToInt(waveTimmerMax / enemySpawnTimerMax);

        if (chosenDifficulty == GameSettings.Difficulty.Easy)
        {
            hopperAmount = Mathf.RoundToInt(numberOfEnemies * 0.5f);

            segwayAmount = Mathf.RoundToInt(numberOfEnemies * 0.3f);

            flyingSauserAmount = Mathf.RoundToInt(numberOfEnemies * 0.2f);

        }
        else if (chosenDifficulty == GameSettings.Difficulty.Medium)
        {
           hopperAmount = Mathf.RoundToInt(numberOfEnemies * 0.35f);

            segwayAmount = Mathf.RoundToInt(numberOfEnemies * 0.4f);

            flyingSauserAmount = Mathf.RoundToInt(numberOfEnemies * 0.25f);

        }
        else if (chosenDifficulty == GameSettings.Difficulty.Hard)
        {
            hopperAmount = Mathf.RoundToInt(numberOfEnemies * 0.15f);

            segwayAmount = Mathf.RoundToInt(numberOfEnemies * 0.35f);

            flyingSauserAmount = Mathf.RoundToInt(numberOfEnemies * 0.5f);

        }
        if (hopperAmount < 0)
        {
            hopperAmount = 0;
        }
        if(segwayAmount < 0)
        {
            segwayAmount = 0;
        }
        if (flyingSauserAmount < 0) 
        {
            flyingSauserAmount = 0;
        }
    }
    public void UpgradedEnemies()
    {

        if (hopperAmount >= Mathf.RoundToInt(numberOfEnemies * 0.2f) && hopperAmount - 2 >=0)
        {
            hopperAmount -=2;
            segwayAmount += 2;
        }
        else if (segwayAmount >= Mathf.RoundToInt(numberOfEnemies * 0.2f) && segwayAmount-2>=0)
        {
            segwayAmount -= 2;
            flyingSauserAmount += 2;
        }






    }
}




