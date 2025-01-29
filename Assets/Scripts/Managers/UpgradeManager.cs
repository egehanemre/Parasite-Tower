using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] public int Money = 10;
    [SerializeField] TextMeshProUGUI moneyText;

    public int[] upgradePrices = { 1, 2, 3, 4, 5 };

    public enum UpgradeLevel
    {
        Tier0,
        Tier1,
        Tier2,
        Tier3,
        Tier4
    }

    [System.Serializable]
    public class TurretData
    {
        public GameObject turret;
        public Button upgradeButton;
        public Image turretImage; // Child image to be colored
        public UpgradeLevel upgradeLevel;
    }

    public List<TurretData> turrets = new List<TurretData>();

    private readonly Dictionary<UpgradeLevel, Color> tierColors = new Dictionary<UpgradeLevel, Color>
    {
        { UpgradeLevel.Tier0, Color.gray },
        { UpgradeLevel.Tier1, Color.white },
        { UpgradeLevel.Tier2, Color.green },
        { UpgradeLevel.Tier3, Color.blue },
        { UpgradeLevel.Tier4, Color.yellow }
    };

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
            UpdateTurretImageColor(turretData);
            ToggleTurretVisibility(turretData); // Set initial visibility
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
        UpdateTurretImageColor(turretData); // Update color after upgrade
        ToggleTurretVisibility(turretData); // Enable/Disable turret

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void UpdateTurretImageColor(TurretData turretData)
    {
        if (turretData.turretImage != null)
        {
            turretData.turretImage.color = tierColors[turretData.upgradeLevel];
        }
    }

    private void ToggleTurretVisibility(TurretData turretData)
    {
        if (turretData.turret != null)
        {
            turretData.turret.SetActive(turretData.upgradeLevel != UpgradeLevel.Tier0);
        }
    }

    public void UpdateMoneyText()
    {
        moneyText.text = "Money: $" + Money;
    }

    private void ApplyTurretUpgrade(TurretData turretData)
    {
        turretData.turret.GetComponent<Turret>().LevelUp();
    }
}
