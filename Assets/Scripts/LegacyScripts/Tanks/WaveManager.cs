using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Wave> waves = new List<Wave>();
    [SerializeField] private int currentWave;
    [SerializeField] private float timeRemaining = 0;


    private void FixedUpdate() {
        timeRemaining -= Time.fixedDeltaTime;
        if (timeRemaining < 0) {
            
            ThrowWave();
        }
    }

    private void ThrowWave() {
        if (waves.Count <= currentWave) {
            timeRemaining = 10;
            return;
        }
        
        Wave wave = waves[currentWave];
        timeRemaining = wave.waveLength + wave.breakLength;
        foreach (var waveSpawn in wave.waveSpawns) {
            foreach (var tank in waveSpawn.tanksToSpawn) waveSpawn.targetLine.SpawnTank(tank);
        }
        currentWave++;
    }
}

[Serializable]
public struct Wave
{
    public int waveLength;
    public int breakLength;
    public List<WaveSpawn> waveSpawns;
}

[Serializable]
public struct WaveSpawn
{
    public TankSpawner targetLine;
    public List<GameObject> tanksToSpawn;
}
