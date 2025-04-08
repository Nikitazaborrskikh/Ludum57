using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerStats : IInitializable, IDisposable
{
    public AudioClip damageSound;
    public AudioClip dieSound;
    public AudioSource audioSource;
    public GameObject managers;

    public event Action<float> OnHealthChanged;
    private float baseMoveSpeed = 5f;
    private float baseFireRate = 0.5f; 
    private float baseDamage = 10f;
    private int baseBulletCount = 1;
    private float baseHealth = 6f;
    private float baseDashDistance = 5f; 

    private float currentMoveSpeed;
    private float currentFireRate;
    private float currentDamage;
    private int currentBulletCount;
    private float currentHealth;
    private float currentDashDistance;

    
    public float MoveSpeed => currentMoveSpeed;
    public float FireRate => currentFireRate;
    public float Damage => currentDamage;
    public int BulletCount => currentBulletCount;
    public float Health => currentHealth;
    public float DashDistance => currentDashDistance; 

    private Dictionary<UpgradeType, List<UpgradeSO>> appliedUpgrades = new();
    
    private Dictionary<string, UpgradeSO> specialEffects = new();

    public Dictionary<string, UpgradeSO> SpecialEffects => specialEffects;
    public PlayerStats()
    {
        appliedUpgrades = new Dictionary<UpgradeType, List<UpgradeSO>>();
        ResetStats(); 
        Debug.Log($"PlayerStats constructed, baseMoveSpeed: {baseMoveSpeed}, currentMoveSpeed: {currentMoveSpeed}");
    }
    public void Initialize()
    {
       
        Debug.Log($"PlayerStats initialized, baseMoveSpeed: {baseMoveSpeed}, currentMoveSpeed: {currentMoveSpeed}");
    }

    public void Dispose()
    {
        ResetStats();
    }

    public void ResetStats()
    {
        currentMoveSpeed = baseMoveSpeed;
        currentFireRate = baseFireRate;
        currentDamage = baseDamage;
        currentBulletCount = baseBulletCount;
        currentHealth = baseHealth;
        currentDashDistance = baseDashDistance;
        appliedUpgrades.Clear();
    }

    public void ApplyUpgrade(UpgradeSO upgrade)
    {
        if (!appliedUpgrades.ContainsKey(upgrade.upgradeType))
        {
            appliedUpgrades[upgrade.upgradeType] = new List<UpgradeSO>();
        }
        appliedUpgrades[upgrade.upgradeType].Add(upgrade);

        if (upgrade.effectType == UpgradeEffectType.StatModifier)
        {
            RecalculateStat(upgrade.upgradeType);
        }
        else if (upgrade.effectType == UpgradeEffectType.SpecialEffect)
        {
            specialEffects[upgrade.specialEffect] = upgrade;
        }

        Debug.Log($"Applied upgrade: {upgrade.upgradeName}, Type: {upgrade.upgradeType}");
    }

    private void RecalculateStat(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.MovementSpeed:
                currentMoveSpeed = CalculateStat(baseMoveSpeed, type);
                break;
            case UpgradeType.FireRate:
                currentFireRate = CalculateStat(baseFireRate, type);
                break;
            case UpgradeType.Damage:
                currentDamage = CalculateStat(baseDamage, type);
                break;
            case UpgradeType.BulletCount:
                currentBulletCount = (int)CalculateStat(baseBulletCount, type);
                break;
            case UpgradeType.Health:
                currentHealth = CalculateStat(baseHealth, type);
                break;
            case UpgradeType.DashDistance: 
                currentDashDistance = CalculateStat(baseDashDistance, type);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            
            audioSource.PlayOneShot(dieSound);
            Die();
            return;
        }
        audioSource.PlayOneShot(damageSound);
    }

    private void Die()
    {
        managers.GetComponent<LevelsManager>().BlinkAndSwitchScene("Menu");
        Dispose();
    }

    private float CalculateStat(float baseValue, UpgradeType type)
    {
        if (!appliedUpgrades.ContainsKey(type)) return baseValue;

        float additiveBonus = 0f;
        float multiplicativeBonus = 1f;

        foreach (var upgrade in appliedUpgrades[type])
        {
           
                additiveBonus += upgrade.value;
        }

        return (baseValue + additiveBonus) * multiplicativeBonus;
    }
}
