using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private int rocketCost = 1;
    [SerializeField] private RocketStock rocketStock;
    [SerializeField] public int Money = 10;
    [SerializeField] TextMeshProUGUI moneyText;

    public int[] upgradePrices = { 1, 3, 5 };

    public enum UpgradeLevel
    {
        Level0,
        Level1,
        Level2,
    }

    [System.Serializable]
    public class TurretData
    {
        public GameObject turret;
        public Button upgradeButton;
        public UpgradeLevel upgradeLevel = UpgradeLevel.Level0;
    }

    public List<TurretData> turrets = new List<TurretData>();
    private void Start()
    {
        UpdateMoneyText();
        InitializeTurretButtons();
    }
    private void InitializeTurretButtons()
    {
        foreach (var turretData in turrets)
        {
            turretData.upgradeButton.onClick.AddListener(() => UpgradeTurret(turretData));
        }
    }

    private void UpgradeTurret(TurretData turretData)
    {
        int currentLevel = (int)turretData.upgradeLevel;

        if (currentLevel >= upgradePrices.Length)
        {
            Debug.Log($"{turretData.turret.name} is already at max level.");
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        int upgradeCost = upgradePrices[currentLevel];
        if (Money < upgradeCost)
        {
            Debug.Log("Not enough money to upgrade.");
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        Money -= upgradeCost;
        UpdateMoneyText();
        turretData.upgradeLevel++;

        Debug.Log($"Upgraded {turretData.turret.name} to {turretData.upgradeLevel}");

        ApplyTurretUpgrade(turretData);

        EventSystem.current.SetSelectedGameObject(null);
    }
    public void UpdateMoneyText()
    {
        moneyText.text = "Money: $" + Money;
    }

    private void ApplyTurretUpgrade(TurretData turretData)
    {
        turretData.turret.GetComponent<Turret>().LevelUp();
    }

    public void BuyRocket()
    {
        if (Money < rocketCost) {
            Debug.Log("Not enough money to upgrade.");
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }
        
        Money -= rocketCost;
        UpdateMoneyText();
        rocketStock.Spawn();
        EventSystem.current.SetSelectedGameObject(null);
    }
}
