using Projectiles;
using UnityEngine;

namespace Enemies.SimpleEnemies
{
    public class UserPage : BaseEnemy
    {
        public override float AttackSpeed => config.userPageStats.attackSpeed;
        public override float DamagePerProjectile => config.userPageStats.damagePerProjectile;
        public override float MovementSpeed => /*PlayerStats.Speed*/ 5f / config.userPageStats.movementSpeedDivider;
        public override float DistanceToPlayer => config.userPageStats.distanceToPlayer;
        
        private ProjectileType projectileType => config.userPageStats.projectileType;
        
        private void Awake()
        {
            Health = config.userPageStats.health;
        }

        public override void Attack(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            for (int i = -1; i <= 1; i += 2)
            {
                Vector3 offset = Vector3.Cross(direction, Vector3.up) * i * 0.5f;
                Projectile projectile = ProjectilePool.Instance.GetProjectile(
                    projectileType, transform.position + offset, Quaternion.identity);
                projectile.GetComponent<Projectile>().Initialize(direction, DamagePerProjectile, projectileType, this);
            }
        }

        public override void Move(Vector3 playerPosition)
        {
            float distance = Vector3.Distance(transform.position, playerPosition);
            if (distance > DistanceToPlayer) // Дистанция атаки
            {
                Vector3 direction = (playerPosition - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);
                transform.position = Vector3.MoveTowards(transform.position,
                    playerPosition,
                    MovementSpeed * Time.deltaTime);
            }
        }
    }
}
