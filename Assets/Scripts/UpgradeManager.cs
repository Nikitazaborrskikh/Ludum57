using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class UpgradeManager : IInitializable
{
    [Inject] private PlayerStats playerStats;
    [Inject] private UpgradeSelectionUI upgradeSelectionUI;
    private List<UpgradeSO> allUpgrades;

    public void Initialize()
    {
        
        allUpgrades = LoadUpgradesFromJson();
        if (allUpgrades == null || allUpgrades.Count == 0)
        {
            Debug.LogError("Failed to load upgrades or no upgrades found!");
            allUpgrades = new List<UpgradeSO>();
        }
    }

    public void OfferUpgrades()
    {
        var appliedUpgrades = playerStats.SpecialEffects.Values.ToList();
        var availableUpgrades = allUpgrades.Where(upgrade => !appliedUpgrades.Any(applied => applied.upgradeName == upgrade.upgradeName)).ToList();

        if (availableUpgrades.Count == 0)
        {
            Debug.Log("No new upgrades available!");
            return;
        }

        UpgradeSO option1 = availableUpgrades[Random.Range(0, availableUpgrades.Count)];
        UpgradeSO option2 = null;
        if (availableUpgrades.Count > 1)
        {
            do
            {
                option2 = availableUpgrades[Random.Range(0, availableUpgrades.Count)];
            } while (option2 == option1);
        }

        upgradeSelectionUI.ShowUpgradeOptions(option1, option2);
    }

    public void ApplyUpgrade(UpgradeSO upgrade)
    {
        playerStats.ApplyUpgrade(upgrade);
        SaveUpgradesToJson();
    }

    private List<UpgradeSO> LoadUpgradesFromJson()
    {
        string json = PlayerPrefs.GetString("Upgrades", "");
        Debug.Log($"Loading upgrades from JSON: {json}");

        if (string.IsNullOrEmpty(json))
        {
            var defaultUpgrades = Resources.LoadAll<UpgradeSO>("Upgrades").ToList();
            Debug.Log($"No saved upgrades, loaded {defaultUpgrades.Count} from Resources.");
            return defaultUpgrades;
        }

        try
        {
            UpgradeData data = JsonUtility.FromJson<UpgradeData>(json);
            if (data == null || data.upgradeNames == null)
            {
                Debug.LogWarning("Invalid JSON data, loading default upgrades.");
                return Resources.LoadAll<UpgradeSO>("Upgrades").ToList();
            }

            var loadedUpgrades = data.ToUpgradeSOList();
            Debug.Log($"Loaded {loadedUpgrades.Count} upgrades from JSON.");
            return loadedUpgrades;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to deserialize JSON: {e.Message}. Loading default upgrades.");
            return Resources.LoadAll<UpgradeSO>("Upgrades").ToList();
        }
    }

    private void SaveUpgradesToJson()
    {
        try
        {
            var upgradesToSave = playerStats.SpecialEffects.Values.Where(u => u != null).ToList();
            UpgradeData data = new UpgradeData(upgradesToSave);
            string json = JsonUtility.ToJson(data);
            Debug.Log($"Saving upgrades to JSON: {json}");
            PlayerPrefs.SetString("Upgrades", json);
            PlayerPrefs.Save(); // Принудительно сохраняем данные
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save upgrades: {e.Message}");
        }
    }
}

[Serializable]
public class UpgradeData
{
    public List<string> upgradeNames;

    public UpgradeData(List<UpgradeSO> upgrades)
    {
        upgradeNames = upgrades.Select(u => u.upgradeName).ToList();
    }

    public List<UpgradeSO> ToUpgradeSOList()
    {
        var allUpgrades = Resources.LoadAll<UpgradeSO>("Upgrades").ToList();
        var result = allUpgrades.Where(u => upgradeNames.Contains(u.upgradeName)).ToList();
        // Проверка на отсутствующие улучшения
        var missingUpgrades = upgradeNames.Where(name => !allUpgrades.Any(u => u.upgradeName == name)).ToList();
        if (missingUpgrades.Any())
        {
            Debug.LogWarning($"Some upgrades not found in Resources: {string.Join(", ", missingUpgrades)}");
        }
        return result;
    }
}