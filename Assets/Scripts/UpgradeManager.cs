using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] public int Money = 10;
    [SerializeField] TextMeshProUGUI moneyText;

    public int[] upgradePrices = { 1, 3, 5 }; 

    public enum UpgradeLevel
    {
        Level0,
        Level1,
        Level2,
        Level3,
    }

    [System.Serializable]
    public class TurretData
    {
        public GameObject turret;
        public Button upgradeButton;
        public TextMeshProUGUI upgradePriceText;
        public GameObject upgradeIndicatorsParent;
        public List<Image> upgradeIndicators;
        public UpgradeLevel upgradeLevel = UpgradeLevel.Level0;
    }

    public List<TurretData> turrets = new List<TurretData>();
    public Sprite upgradeLevelSprite;
    public Sprite noLevelSprite;

    private void Start()
    {
        InitializeTurretButtons();
        InitializeLevelIndicators();
        UpdateMoneyText();
    }

    private void InitializeLevelIndicators()
    {
        foreach (var turretData in turrets)
        {
            turretData.upgradeIndicators.Clear();

            foreach (Transform child in turretData.upgradeIndicatorsParent.transform)
            {
                Image indicator = child.GetComponent<Image>();
                if (indicator != null)
                {
                    turretData.upgradeIndicators.Add(indicator);
                }
            }
            UpdateUpgradeUI(turretData);
        }
    }

    private void InitializeTurretButtons()
    {
        foreach (var turretData in turrets)
        {
            turretData.upgradeButton.onClick.AddListener(() => UpgradeTurret(turretData));
            UpdateUpgradeUI(turretData);
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
        UpdateUpgradeUI(turretData);

        ApplyTurretUpgrade(turretData);

        EventSystem.current.SetSelectedGameObject(null);
    }
    private void UpdateUpgradeUI(TurretData turretData)
    {
        int currentLevel = (int)turretData.upgradeLevel;

        for (int i = 0; i < turretData.upgradeIndicators.Count; i++)
        {
            turretData.upgradeIndicators[i].sprite = i < currentLevel ? upgradeLevelSprite : noLevelSprite;
        }

        if (currentLevel < upgradePrices.Length)
        {
            int nextUpgradeCost = upgradePrices[currentLevel];
            turretData.upgradePriceText.text = (currentLevel == 0 ? "Purchase: $" : "Upgrade: $") + nextUpgradeCost;
            turretData.upgradeButton.interactable = Money >= nextUpgradeCost;
        }
        else
        {
            turretData.upgradePriceText.text = "MAX";
            turretData.upgradeButton.interactable = false;
        }
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "Money: $" + Money;
    }

    private void ApplyTurretUpgrade(TurretData turretData)
    {
        //Todo: Apply upgrade to turret
    }
}
