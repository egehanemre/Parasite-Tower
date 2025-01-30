using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketStock : MonoBehaviour
{
    [SerializeField] private GameObject rocketAmmunitionPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int cost = 1;
    private UpgradeManager upgradeManager;

    private void Start() {
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }
    
    public void Spawn() {
        if(upgradeManager.Money < cost) return;

        upgradeManager.Money -= cost;
        upgradeManager.UpdateMoneyText();
        Instantiate(rocketAmmunitionPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
