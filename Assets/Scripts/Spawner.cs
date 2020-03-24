using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;

    private Wave currentWave;
    private int currentWaveNumber = 0;

    private int enemiesRemainingToSpawn;
    private int enemiesRemainingAlive;
    private float nextSpawnTime;

    void Start()
    {
        NextWave();
    }

    void Update()
    {
        if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity);
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if(enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWaveNumber++;
        if(currentWaveNumber - 1 < waves.Length)
        {
            print("Wave: " + currentWaveNumber);
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }        
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;



    }
}
