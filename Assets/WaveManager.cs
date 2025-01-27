using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int waveNumber = 0; 
    public int baseEnemyCount = 5;
    public float enemyIncreaseFactor = 1.5f;

    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs; 

    [Header("Spawn Settings")]
    public Transform targetPosition;
    public float spawnRadius = 10f; 
    public float spawnDelay = 0.5f;

    private List<GameObject> spawnedEnemies = new List<GameObject>(); 

    void Start()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        waveNumber++;
        int enemyCount = Mathf.CeilToInt(baseEnemyCount * Mathf.Pow(enemyIncreaseFactor, waveNumber - 1));
        StartCoroutine(SpawnWave(enemyCount));
    }

    private IEnumerator SpawnWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;

        float temporaryYFix = 1f;
        Vector3 spawnPosition = new Vector3(targetPosition.position.x + randomOffset.x, targetPosition.position.y + temporaryYFix, targetPosition.position.z + randomOffset.y);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(spawnedEnemy);
    }

    public void ClearEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }
}
