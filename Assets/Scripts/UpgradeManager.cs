using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using Random = UnityEngine.Random;

public class UpgradeManager : IInitializable
{
    [Inject] private PlayerStats playerStats;
    [Inject] private UpgradeSelectionUI upgradeSelectionUI;
    private List<UpgradeSO> allUpgrades; // Загружается из JSON

    public void Initialize()
    {
        allUpgrades = LoadUpgradesFromJson();
    }

    public void OfferUpgrades()
    {
        UpgradeSO option1 = allUpgrades[Random.Range(0, allUpgrades.Count)];
        UpgradeSO option2 = allUpgrades[Random.Range(0, allUpgrades.Count)];
        while (option2 == option1) option2 = allUpgrades[Random.Range(0, allUpgrades.Count)];
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
        if (string.IsNullOrEmpty(json)) return Resources.LoadAll<UpgradeSO>("Upgrades").ToList();
        return JsonUtility.FromJson<UpgradeData>(json).ToUpgradeSOList();
    }

    private void SaveUpgradesToJson()
    {
        UpgradeData data = new UpgradeData(playerStats.SpecialEffects.Values.ToList());
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("Upgrades", json);
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
        return allUpgrades.Where(u => upgradeNames.Contains(u.upgradeName)).ToList();
    }
}