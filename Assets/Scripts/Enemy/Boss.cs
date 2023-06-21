using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float spawnRadius = 10f;
    public int maxEnemies = 5;

    public float bossCurrentHP;

    public int hpThreshold = 100;

    public int activeEnemies = 0;

    private void Start()
    {
        bossCurrentHP = gameObject.GetComponent<Enemy>().currentHealth;
    }

    private void Update()
    {
        if (bossCurrentHP - gameObject.GetComponent<Enemy>().currentHealth >= hpThreshold)
        {
            SpawnEnemies();
            bossCurrentHP = gameObject.GetComponent<Enemy>().currentHealth;
        }
    }

    private void SpawnEnemies()
    {
        activeEnemies = 0;
        while (activeEnemies < maxEnemies)
        {           
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Vector3 spawnPosition = GetRandomSpawnPosition();

                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            activeEnemies++;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + gameObject.transform.position;
        return spawnPosition;
    }

}
