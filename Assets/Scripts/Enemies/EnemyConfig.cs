using Projectiles;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Enemy/EnemyConfig", order = 1)]
    public class EnemyConfig : ScriptableObject
    {
        [System.Serializable]
        public class EnemyStats
        {
            public ProjectileType projectileType;
            public GameObject projectilePrefab; // Добавляем префаб снаряда
            public float attackSpeed;
            public float damagePerProjectile;
            public float movementSpeedDivider;
            public float distanceToPlayer;
            public float health;
        }

        [System.Serializable]
        public class TwoFactorAuthStats
        {
            public EnemyStats phase1;
            public EnemyStats phase2;
        }

        public EnemyStats captchaStats;
        public EnemyStats userPageStats;
        public EnemyStats firewallStats;

        public EnemyStats encryptionStats;
        public TwoFactorAuthStats twoFactorAuth;
        public EnemyStats backupStats;
    }
}