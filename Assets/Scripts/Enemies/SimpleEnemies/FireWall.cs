using Projectiles;
using UnityEngine;
using Zenject;

namespace Enemies.SimpleEnemies
{
    public class Firewall : BaseEnemy
    {
        public override float AttackSpeed => config.firewallStats.attackSpeed;
        public override float DamagePerProjectile => config.firewallStats.damagePerProjectile;
        public override float MovementSpeed => /*PlayerStats.Speed*/ 5f / config.firewallStats.movementSpeedDivider;
        public override float DistanceToPlayer => config.firewallStats.distanceToPlayer;
        
        private ProjectileType projectileType => config.firewallStats.projectileType;
        private Rigidbody rb;
        
        [Inject] private UpgradeManager upgradeManager;
        private void Awake()
        {
            Health = config.firewallStats.health;
            rb = GetComponent<Rigidbody>();
        }

        public override void Attack(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            for (int i = -1; i <= 1; i++)
            {
                Vector3 spread = Quaternion.Euler(0, 15 * i, 0) * direction;
                Projectile projectile = ProjectilePool.Instance.GetProjectile(
                    projectileType, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile>().Initialize(spread, DamagePerProjectile, projectileType, this);
            }
        }

        public override void Move(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            
            float distance = Vector3.Distance(transform.position, playerPosition);
            if (distance > DistanceToPlayer) // Дистанция атаки
            {
                rb.constraints = RigidbodyConstraints.None;
                transform.position = Vector3.MoveTowards(transform.position,
                    playerPosition,
                    MovementSpeed * Time.deltaTime);
            }
            else rb.constraints = RigidbodyConstraints.FreezePosition;
        }
    }
}
