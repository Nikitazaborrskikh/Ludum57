using UnityEngine;

public enum UpgradeType
{
    MovementSpeed,
    FireRate,
    Damage,
    BulletCount,
    Health,
    DashDistance,
    // Добавляйте новые типы по мере необходимости
}
public enum UpgradeEffectType
{
    StatModifier,      // Простое изменение характеристик
    SpecialEffect      // Специальный эффект (рикошет, одновременные атаки и т.д.)
}

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Base Upgrade")]
public class UpgradeSO : ScriptableObject
{
    public string upgradeName;
    public string upgradeTypeKey;
    public string description;
    public UpgradeType upgradeType;
    public float value;              // Для числовых изменений
    public UpgradeEffectType effectType;
    public bool isPrimaryAttack;     // Для ЛКМ (true) или ПКМ (false)
    public string specialEffect;     // Уникальный идентификатор эффекта (например, "Ricochet", "ConeShot")
}
