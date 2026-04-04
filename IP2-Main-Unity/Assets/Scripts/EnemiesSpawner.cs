
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
    public int maxWaves = 10;
    public float waveTimmerMax;
    public float enemySpawnTimerMax;
    public float waitTimerMax;

    [Tooltip("Delay between rounds")]
    public float waitTimer;
    public float waveTimmer;
    public float enemySpawnTimer;

    public bool InWave;
    public float initialPause;
    

    public List<Vector3> pathWaypoint = new List<Vector3>();
    public GameObject[] enemies;

    [Header("Rocket Spawn Animation Stuff")]
    public GameObject rocketWithStairs; //Rocket with the stairs model
    public GameObject flyingRocket; //The rocket that actually flies down
    public float rocketSpeed; //Calculated using RocketScience() Method
    public float groundLevel; //Put slightly higher than the actual ground so that the rocket doesn't clip through

    private void Start()
    {
        InWave = false;
        waveNumber = 0;
        waitTimer = 0;
        waveTimmer = 0;
        enemySpawnTimer = 0;

        rocketWithStairs.SetActive(false);
        flyingRocket.SetActive(true);

        EnemiesAmount();
        for (int i = 0; i < pathWaypoint.Count; i++)// sets postitions for astronaught path
        {
            pathWaypoint[i] = new Vector3(
                 pathWaypoint[i].x,
                 transform.position.y,
                 pathWaypoint[i].z
                );
        }

        chosenDifficulty = GameSettings.chosenDifficulty;

        RocketScience();
    }

    private void Update()
    {
        if (InWave)
        {
            enemySpawnTimer = enemySpawnTimer + Time.deltaTime;
            waveTimmer = waveTimmer + Time.deltaTime;
        }
        else if (initialPause > 0) //This is maybe an insane way to do all this but it works so..
        {
            initialPause = initialPause - Time.deltaTime;

            //ROCKET SPAWN ANIMATION STUFF - In this bit because no enemies spawn in yet so allows the rocket to land and then eventually spawns enemies

            Vector3 targetRocketPos = new Vector3(flyingRocket.transform.position.x, groundLevel, flyingRocket.transform.position.z);

            flyingRocket.transform.position = Vector3.MoveTowards(flyingRocket.transform.position, targetRocketPos, rocketSpeed * Time.deltaTime);
            //Line above just moves the flying rocket towards the ground

            if (flyingRocket.transform.position.y <= (groundLevel + 0.31f))
            {
                //LandedRocket();
                //Used to call LandedRocket() in this script, however now gets called in GameUI script so that the wave banner covers the switch between models
            }
        }
        else if (initialPause < 0) //Gives an initial pause before the first wave starts that only ever runs 1 time
        {
            waitTimer = waitTimerMax;
            initialPause = 0;
        }
        else
        {
            waitTimer = waitTimer + Time.deltaTime;
        }

        SpawnEnemies();
        EndOfWave();
    }

    public void EndOfWave()
    {
        if (waveTimmer > waveTimmerMax && InWave)
        {
            InWave = false;
            waitTimer = 0f;

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
                enemySpawnTimerMax = Mathf.Max(1f, enemySpawnTimerMax - 0.7f); //Reduces spawn time by 0.7 to a min of 1
                EnemiesAmount();
            }
            else if (upgradedEnemies)
            {
                UpgradedEnemies();
            }
        }

        if (waitTimer > waitTimerMax)
        {
            if (waveNumber >= maxWaves)
            {
                Debug.Log("sucessfully finished"); //Last wave has been, disable the spawner
                enabled = false;
                return;
            }




            waveNumber++;
            InWave = true;
            waveTimmer = 0f;
            waitTimer = 0f;
            enemySpawnTimer = 0f;


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

            enemySpawnTimer = 0f;
        }
    }
    public void EnemiesAmount()
    {

        numberOfEnemies = Mathf.RoundToInt(waveTimmerMax / enemySpawnTimerMax);

        if (chosenDifficulty == GameSettings.Difficulty.Easy)
        {
            hopperAmount = Mathf.RoundToInt(numberOfEnemies * 0.6f);

            segwayAmount = Mathf.RoundToInt(numberOfEnemies * 0.25f);

            flyingSauserAmount = Mathf.RoundToInt(numberOfEnemies * 0.15f);

        }
        else if (chosenDifficulty == GameSettings.Difficulty.Medium)
        {
           hopperAmount = Mathf.RoundToInt(numberOfEnemies * 0.5f);

            segwayAmount = Mathf.RoundToInt(numberOfEnemies * 0.3f);

            flyingSauserAmount = Mathf.RoundToInt(numberOfEnemies * 0.2f);

        }
        else if (chosenDifficulty == GameSettings.Difficulty.Hard)
        {
            hopperAmount = Mathf.RoundToInt(numberOfEnemies * 0.35f);

            segwayAmount = Mathf.RoundToInt(numberOfEnemies * 0.35f);

            flyingSauserAmount = Mathf.RoundToInt(numberOfEnemies * 0.3f);

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

    public void RocketScience()
    {
        //Not actually rocket science, just uses the Speed = Distance/Time formula to work out the
        //speed needed for the rocket to actually land during the prep phase
        float distanceToTravel = flyingRocket.transform.position.y - groundLevel; //Works out the distance
        rocketSpeed = distanceToTravel / initialPause; //Speed = Distance/Time
    }
    public void LandedRocket()
    {
        //Swaps the model for the rocket
        flyingRocket.SetActive(false);
        rocketWithStairs.SetActive(true);
    }
}




