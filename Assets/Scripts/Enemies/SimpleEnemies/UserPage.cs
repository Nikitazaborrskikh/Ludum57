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
        private Rigidbody rb;

        private void Awake()
        {
            Health = config.userPageStats.health;
            rb = GetComponent<Rigidbody>();
        }

        public override void Attack(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            for (int i = -1; i <= 1; i += 2)
            {
                Vector3 offset = Vector3.Cross(direction, Vector3.up) * i * 0.5f;
                Projectile projectile = ProjectilePool.Instance.GetProjectile(
                    GetProjectilePrefab(),
                    projectileType,
                    transform.position + offset,
                    rotation
                );
                projectile.Initialize(GetProjectilePrefab(), direction, rotation, DamagePerProjectile, projectileType, this);
            }
        }

        public override void Move(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);

            float distance = Vector3.Distance(transform.position, playerPosition);
            if (distance > DistanceToPlayer)
            {
                rb.constraints = RigidbodyConstraints.None;
                transform.position = Vector3.MoveTowards(transform.position,
                    playerPosition,
                    MovementSpeed * Time.deltaTime);
            }
            else rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        protected override GameObject GetProjectilePrefab()
        {
            return config.userPageStats.projectilePrefab;
        }

        protected override ProjectileType GetProjectileType()
        {
            return config.userPageStats.projectileType;
        }
    }
}