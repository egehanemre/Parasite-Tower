using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum SpawnDirection { North, South, East, West }

[System.Serializable]
public class DirectionalTankSpawn
{
    public SpawnDirection direction; // Enum instead of string
    public GameObject tankPrefab;
    public int count;
}

[System.Serializable]
public class SubWave
{
    public List<DirectionalTankSpawn> tankSpawns; // Each entry defines tanks per direction
}

[System.Serializable]
public class Wave
{
    public List<SubWave> subWaves; // A wave consists of multiple sub-waves
}

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWaveIndex = 0;
    public List<Wave> waves;

    [Header("Spawn Settings")]
    public Transform northSpawn;
    public Transform southSpawn;
    public Transform eastSpawn;
    public Transform westSpawn;
    public float spawnDelay = 1f;
    public float spawnRadius = 5f; // Circle radius (10x10 area)

    [Header("UI Elements")]
    public TextMeshProUGUI waveText;

    private Dictionary<SpawnDirection, Transform> spawnPoints;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        // Assign spawn points using enums
        spawnPoints = new Dictionary<SpawnDirection, Transform>()
        {
            { SpawnDirection.North, northSpawn },
            { SpawnDirection.South, southSpawn },
            { SpawnDirection.East, eastSpawn },
            { SpawnDirection.West, westSpawn }
        };

        StartNextWave();
    }

    public void StartNextWave()
    {
        if (currentWaveIndex >= waves.Count) return;

        Wave currentWave = waves[currentWaveIndex];

        if (waveText != null)
            waveText.text = $"Wave {currentWaveIndex + 1}";

        StartCoroutine(SpawnWave(currentWave));
        currentWaveIndex++;
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        foreach (SubWave subWave in wave.subWaves)
        {
            foreach (DirectionalTankSpawn tankSpawn in subWave.tankSpawns)
            {
                if (!spawnPoints.TryGetValue(tankSpawn.direction, out Transform spawnTransform) || spawnTransform == null)
                {
                    Debug.LogWarning($"Missing spawn point for direction: {tankSpawn.direction}");
                    continue;
                }

                for (int i = 0; i < tankSpawn.count; i++)
                {
                    Vector3 spawnPos = GetRandomSpawnPosition(spawnTransform.position);
                    GameObject spawnedTank = Instantiate(tankSpawn.tankPrefab, spawnPos, Quaternion.identity);
                    spawnedEnemies.Add(spawnedTank);
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
        }
    }
    private Vector3 GetRandomSpawnPosition(Vector3 spawnPointPosition)
    {
        float minRadius = 100f; //this should be the radius of circle we have with transforms in N E S W 
        float maxRadius = 115f;

        Vector3 direction = (spawnPointPosition - Vector3.zero).normalized;

        float distance = Random.Range(minRadius, maxRadius);

        float angleOffset = Random.Range(-30f, 30f); 

        Quaternion rotation = Quaternion.Euler(0, angleOffset, 0);
        Vector3 rotatedDirection = rotation * direction;

        Vector3 spawnPosition = rotatedDirection * distance;
        spawnPosition.y = spawnPointPosition.y; 

        return spawnPosition;
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
