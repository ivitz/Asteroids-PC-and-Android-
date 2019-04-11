using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour {

    //Road placement information
    [Header("Road Spawn---")]
    [Tooltip("In this case 30 (each road block distance is 30 units). This will be added to place next block of road")]
    public float roadStep;                     
    [Tooltip("Set this to the Z position of the last road prefab in the scene")]
    public float roadEnd;                
    private float nextRoadZ;       //For example 90 (previous road was at 60, next is 60 + roadStep)

    [Header("Asteroid Spawn---")]
    public float spawnRate = 10f;
    public float spawnRateDropSpeed = 0.001f;
    public float spawnRateDeceleration = 0.001f;
    public float minSpawnRate = 1f;
    private float lastSpawnTime = 0;
    private float randomTime = 0;

    //Distance traveled
    private float shipPositionZ;                //Use this to calculate the distance
    private float previousShipPosition = 0;

    //ship details
    private Transform ship;

    [Header("Object Poolers")]
    public ObjectPooler asteroidPool;
    public ObjectPooler roadPool;

	void Start () 
    {        
        ship = GameObject.FindWithTag("Player").transform;
        lastSpawnTime = 0;
        //StartCoroutine(AsteroidPlacement());
	}	
	
	void Update () 
    {
        if (!StaticVariables.gamePaused)
        {
            // Calculate the current ship position
            shipPositionZ = ship.position.z;    

            RoadPlacement();
            AsteroidCalculations();
        }
	}

    //Place new road block
    private void RoadPlacement ()
    {      
        //-4.5f correction to get the actual start of the road block
        if (shipPositionZ >= previousShipPosition + roadStep - 4.5f)
        {
            //Get the position of the ship in current frame and don't update it till next road placement
            previousShipPosition = shipPositionZ;

            //Calculate the place where the next road block will be placed (current road end + block lenght)
            nextRoadZ = roadEnd + roadStep;
            Vector3 newRoadPosition = new Vector3(0, -1, nextRoadZ);

            //Get the available road block from the pool, enable it and place correctly
            GameObject road = roadPool.GetObject();
            road.transform.position = newRoadPosition;
            road.transform.rotation = Quaternion.identity;
            road.SetActive(true);

            //calculate the next road Z position
            roadEnd = nextRoadZ;
        }
    }

    //this will calculate if it time to spawn an asteroid and call Spawn method.
    private void AsteroidCalculations()
    {  
        //RateDropSpeed also slightly depends on the score
        spawnRateDropSpeed += spawnRateDeceleration * (StaticVariables.gameManager.currentScore / 1000) * Time.deltaTime;

        spawnRate = Mathf.Max(spawnRate - Time.deltaTime * spawnRateDropSpeed, minSpawnRate);

        if(Time.time - lastSpawnTime > spawnRate + randomTime)
        {
            //Add some random delay to spawning
            randomTime = Random.Range(0, 1.3f);

            lastSpawnTime = Time.time;

            //Spawn the asteroid
            AsteroidSpawn();
        }
    }

    private void AsteroidSpawn()
    {
        //Get the random position for asteroid to spawn at
        float rndX = Random.Range(-5, 5);
        Vector3 newAsteroidPosition = new Vector3(rndX, 1.2f, ship.position.z + 190);

        //Get asteroid from pool and place correctly
        GameObject obj = asteroidPool.GetObject();
        obj.transform.position = newAsteroidPosition;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }
}















    ////////////////////////////Deprecated!
    /// I used coroutine to spawn asteroid. Decided to make the process more flexible.
    ////////////////////////////Deprecated!

    /*
    //Use Coroutine to spawn the asteroids
    //Calculate wait time
    //If not paused or Game over - spawn
    //Get the random location to spawn
    private IEnumerator AsteroidPlacement ()
    {
        while (!StaticVariables.gameManager.gameOver)
        {
            yield return new WaitForSeconds(CalculateAsteroidSpawnTime());
            if (!StaticVariables.gameManager.gameOver)
            {
                float rndX = Random.Range(-5, 5);
                Vector3 newAsteroidPosition = new Vector3(rndX, 1.2f, ship.position.z + 190);

                //Get asteroid from pool and place correctly
                GameObject obj = asteroidPool.GetObject();
                obj.transform.position = newAsteroidPosition;
                obj.transform.rotation = Quaternion.identity;
                obj.SetActive(true);

            }
        }
    }

    //Calculate the time for the next Asteroid to spawn
    private float CalculateAsteroidSpawnTime()
    {
        float nextSpawn;

        float min = 1 - (StaticVariables.gameManager.currentScore * 0.99f / 100);        
        float max = 3 - (StaticVariables.gameManager.currentScore * 0.99f / 100);       

        nextSpawn = Random.Range(min, max);

        if (nextSpawn < 0.4f)
            nextSpawn = 0.4f;          

        return nextSpawn;
    }
    */

