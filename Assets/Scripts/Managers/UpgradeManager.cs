using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] public int Money = 10;
    [SerializeField] TextMeshProUGUI moneyText;

    public int[] upgradePrices = { 1, 1, 1, 1 };

    public enum UpgradeLevel
    {
        Tier0,
        Tier1,
        Tier2,
        Tier3,
        Tier4
    }

    private UpgradeLevel maxLevel = UpgradeLevel.Tier4;

    [System.Serializable]
    public class TurretData
    {
        public GameObject turret;
        public Button upgradeButton;
        public Image turretImage; // Child image to be colored
        public UpgradeLevel upgradeLevel;
        public TextMeshProUGUI tierText;
        public TextMeshProUGUI upgradeCostText;
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

    private readonly Dictionary<UpgradeLevel, string> romanTiers = new Dictionary<UpgradeLevel, string>
    {
        { UpgradeLevel.Tier0, "0" },
        { UpgradeLevel.Tier1, "I" },
        { UpgradeLevel.Tier2, "II" },
        { UpgradeLevel.Tier3, "III" },
        { UpgradeLevel.Tier4, "IV" }
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
            ToggleTurretVisibility(turretData);
            UpdateTierText(turretData); // Set initial tier text
            UpdateUpgradeCostText(turretData);
        }
    }

    private void UpgradeTurret(TurretData turretData)
    {
        int currentLevel = (int)turretData.upgradeLevel;

        if (turretData.upgradeLevel >= maxLevel) // Prevent overflow
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

        ApplyTurretUpgrade(turretData);
        UpdateTurretImageColor(turretData);
        ToggleTurretVisibility(turretData);
        UpdateTierText(turretData); // Update UI tier text
        UpdateUpgradeCostText(turretData);

        EventSystem.current.SetSelectedGameObject(null);
        TutorialSignalManager.PushSignal(SignalType.Upgrade, false);
    }

    private void UpdateTurretImageColor(TurretData turretData)
    {
        if (turretData.turretImage != null && turretData.upgradeLevel <= UpgradeLevel.Tier4)
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

    private void UpdateTierText(TurretData turretData)
    {
        if (turretData.tierText != null)
        {
            turretData.tierText.text = "Tier " + romanTiers[turretData.upgradeLevel];
        }
    }

    private void UpdateUpgradeCostText(TurretData turretData)
    {
        if (turretData.upgradeCostText != null)
        {
            int currentLevel = (int)turretData.upgradeLevel;
            if (turretData.upgradeLevel == UpgradeLevel.Tier0)
            {
                turretData.upgradeCostText.text = "Build: $" + upgradePrices[currentLevel];
            }
            else if(turretData.upgradeLevel != UpgradeLevel.Tier4)
            {
                turretData.upgradeCostText.text = "TierUp: $" + upgradePrices[currentLevel];
            }
            else if(turretData.upgradeLevel == UpgradeLevel.Tier4)
            {
                turretData.upgradeCostText.text = "MAX TIER";
            }
        }
    }
}
