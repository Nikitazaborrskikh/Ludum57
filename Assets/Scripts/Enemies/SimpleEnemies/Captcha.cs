using Projectiles;
using UnityEngine;
using Zenject;

namespace Enemies.SimpleEnemies
{
    public class Captcha : BaseEnemy
    {
        public override float AttackSpeed => config.captchaStats.attackSpeed;
        public override float DamagePerProjectile => config.captchaStats.damagePerProjectile;
        public override float MovementSpeed => /*PlayerStats.Speed*/ 5f / config.captchaStats.movementSpeedDivider;
        public override float DistanceToPlayer => config.captchaStats.distanceToPlayer;

        private ProjectileType projectileType => config.captchaStats.projectileType;
        private Rigidbody rb;

        private void Awake()
        {
            Health = config.captchaStats.health;
            rb = GetComponent<Rigidbody>();
        }

        public override void Attack(Vector3 playerPosition)
        {
            StartCoroutine(AttackSequence(playerPosition));
        }

        private System.Collections.IEnumerator AttackSequence(Vector3 playerPosition)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 direction = (playerPosition - transform.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(direction);
                Projectile projectile = projectilePool.GetProjectile(
                    GetProjectilePrefab(),
                    projectileType,
                    transform.position,
                    rotation // Используем rotation вместо Quaternion.identity для направления
                );
                projectile.Initialize(GetProjectilePrefab(), direction, rotation, DamagePerProjectile, projectileType, this);
                yield return new WaitForSeconds(0.2f);
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
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePosition;
            }
        }

        protected override GameObject GetProjectilePrefab()
        {
            return config.captchaStats.projectilePrefab;
        }

        protected override ProjectileType GetProjectileType()
        {
            return config.captchaStats.projectileType;
        }
    }
}