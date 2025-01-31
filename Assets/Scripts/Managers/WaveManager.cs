using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum SpawnDirection { North, South, East, West }

[System.Serializable]
public class DirectionalTankSpawn
{
    public SpawnDirection direction;
    public GameObject tankPrefab;
    public int count;
}

[System.Serializable]
public class SubWave
{
    public List<DirectionalTankSpawn> tankSpawns;
    public float subWaveTimer;
    [HideInInspector] public bool isFinalSubWave;

    public void SetFinalSubWave(bool isFinal)
    {
        isFinalSubWave = isFinal;
    }
}

[System.Serializable]
public class Wave
{
    public List<SubWave> subWaves;

    public void UpdateSubWaves()
    {
        if (subWaves.Count > 0)
        {
            for (int i = 0; i < subWaves.Count; i++)
            {
                subWaves[i].SetFinalSubWave(i == subWaves.Count - 1);
            }
        }
    }
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

    [Header("UI Elements")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI enemiesRemainingText;

    private Dictionary<SpawnDirection, Transform> spawnPoints;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        spawnPoints = new Dictionary<SpawnDirection, Transform>()
        {
            { SpawnDirection.North, northSpawn },
            { SpawnDirection.South, southSpawn },
            { SpawnDirection.East, eastSpawn },
            { SpawnDirection.West, westSpawn }
        };

        StartCoroutine(StartNextWave());
    }

    private void OnValidate()
    {
        foreach (var wave in waves)
        {
            wave.UpdateSubWaves();
        }
    }

    private IEnumerator StartNextWave()
    {
        while (currentWaveIndex < waves.Count)
        {
            Wave wave = waves[currentWaveIndex];

            // Show wave starting countdown only for the first subwave
            yield return StartCoroutine(DisplayCountdown($"Next Wave: {currentWaveIndex + 1}.0", 15f));

            // Start the wave and wait for it to finish before incrementing the index
            yield return StartCoroutine(SpawnWave(wave));

            currentWaveIndex++;
        }

        cooldownText.text = "All waves completed!";
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.subWaves.Count; i++)
        {
            SubWave subWave = wave.subWaves[i];

            // Update wave text dynamically
            waveText.text = $"Current Wave: {currentWaveIndex + 1}.{i}";

            // Spawn the subwave
            StartCoroutine(SpawnSubWave(subWave));

            // Wait for the subwave timer before starting the next subwave
            yield return StartCoroutine(DisplayCountdown($"Next Wave: {currentWaveIndex + 1}.{i + 1}", subWave.subWaveTimer));
        }
    }

    private IEnumerator SpawnSubWave(SubWave subWave)
    {
        foreach (DirectionalTankSpawn tankSpawn in subWave.tankSpawns)
        {
            if (!spawnPoints.TryGetValue(tankSpawn.direction, out Transform spawnTransform) || spawnTransform == null)
            {
                Debug.LogWarning($"Missing spawn point for direction: {tankSpawn.direction}");
                continue;
            }

            for (int j = 0; j < tankSpawn.count; j++)
            {
                Vector3 spawnPos = GetRandomSpawnPosition(spawnTransform.position);
                GameObject spawnedTank = Instantiate(tankSpawn.tankPrefab, spawnPos, Quaternion.identity);
                spawnedEnemies.Add(spawnedTank);
                UpdateEnemyCount();
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }

    private IEnumerator DisplayCountdown(string message, float time)
    {
        while (time > 0)
        {
            cooldownText.text = $"{message} in {time:F1} sec";
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }
        cooldownText.text = "";
    }

    private Vector3 GetRandomSpawnPosition(Vector3 spawnPointPosition)
    {
        float minRadius = 100f;
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
        UpdateEnemyCount();
    }

    private void UpdateEnemyCount()
    {
        if (enemiesRemainingText != null)
            enemiesRemainingText.text = $"Enemies Left: {spawnedEnemies.Count}";
    }

    private void Update()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);
        UpdateEnemyCount();
    }
}
