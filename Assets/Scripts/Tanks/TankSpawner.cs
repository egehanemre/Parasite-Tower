using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TankSpawner : MonoBehaviour, IEvent
{
    [SerializeField] private List<TankSpawnSlot> spawnSlots = new List<TankSpawnSlot>();
    [SerializeField] private GameObject tankPrefab;
    [SerializeField] private float spawnFrequency = 0.1f;
    [SerializeField] private float slotsSpawnCooldown = 5f;
    [SerializeField] private Vector3 posVariety = new Vector3();
    [SerializeField] private Transform designatedTarget;
    [SerializeField] private int maxTankPerLine = 2;

    public void FixedUpdate()
    {
        if (spawnSlots == null || spawnSlots.Count == 0) {
            Debug.Log("No spawn slots for tank spawner found.");
            return;
        }
        
        foreach (var spawnSlot in spawnSlots) {
            spawnSlot.cooldown -= Time.fixedDeltaTime;
        }
    }

    public void Tick()
    {
        TankSpawnSlot foundSlot = FindSpawnSlot();
        if(foundSlot != null) SpawnTankAtSlot(foundSlot, designatedTarget);
    }

    public float GetWeight() {
        return spawnFrequency;
    }

    public void SpawnTankAtSlot(TankSpawnSlot slot, Transform target) {
        slot.cooldown = slotsSpawnCooldown;
        
        GameObject spawnedTank = Instantiate(tankPrefab, slot.location);
        spawnedTank.transform.localPosition = Vector3.zero;
        
        spawnedTank.transform.position += new Vector3(Random.Range(-posVariety.x, posVariety.x),
            Random.Range(-posVariety.y, posVariety.y), Random.Range(-posVariety.z, posVariety.z));

        if (spawnedTank.TryGetComponent<Tank>(out Tank tank)) {
            tank.Setup(target);
            slot.spawnedTanks.Add(tank);
        }
    }

    public TankSpawnSlot FindSpawnSlot() {
        if (spawnSlots == null || spawnSlots.Count == 0)
        {
            Debug.Log("No spawn slots for tank spawner found.");
            return null;
        }
        
        foreach (var spawnSlot in spawnSlots)
        {
            if(spawnSlot.cooldown > 0 || spawnSlot.spawnedTanks.Count >= maxTankPerLine) continue;
            return spawnSlot;
        }

        Debug.Log("All spawn slots are already occupied.");
        return null;
    }
}

[Serializable]
public class TankSpawnSlot
{
    public float cooldown = 1;
    public Transform location;
    public List<Tank> spawnedTanks = new List<Tank>();
}
