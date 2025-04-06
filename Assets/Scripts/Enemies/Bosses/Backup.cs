using System.Collections;
using Projectiles;
using UnityEngine;

namespace Enemies.Bosses
{
    public class Backup : BaseEnemy
    {
        public override float AttackSpeed => config.backupStats.attackSpeed;
        public override float DamagePerProjectile => config.backupStats.damagePerProjectile;
        public override float MovementSpeed => /*PlayerStats.Speed*/ 5f / config.backupStats.movementSpeedDivider;
        public override float DistanceToPlayer => config.backupStats.distanceToPlayer;
        
        private ProjectileType projectileType => config.backupStats.projectileType;
        
        private float spawnTimer;
        public GameObject firewallPrefab;

        private void Awake()
        {
            Health = config.backupStats.health;
        }
        
        private void Start()
        {
            StartCoroutine(SpawnFirewallRoutine()); // Запускаем корутину при старте
        }

        public override void Attack(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            float angleStep = 15f; // Шаг угла между снарядами
            float startAngle = -angleStep * 2.5f; // Начальный угол для 6 снарядов: -37.5°

            for (int i = 0; i < 6; i++)
            {
                float angle = startAngle + (i * angleStep); // -37.5°, -22.5°, -7.5°, 7.5°, 22.5°, 37.5°
                Vector3 spread = Quaternion.Euler(0, angle, 0) * direction;
                Projectile projectile = ProjectilePool.Instance.GetProjectile(
                    projectileType, transform.position, Quaternion.identity);
                projectile.Initialize(spread, DamagePerProjectile, projectileType, this);
                projectile.OnHit += HandleProjectileHit;
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
        
        private IEnumerator SpawnFirewallRoutine()
        {
            while (true) // Бесконечный цикл для постоянного спавна
            {
                yield return new WaitForSeconds(Random.Range(10f, 15f)); // Ждем 10–15 секунд
                SpawnFirewalls();
            }
        }

        private void SpawnFirewalls()
        {
            if (firewallPrefab == null)
            {
                Debug.LogError("Firewall prefab is not assigned!");
                return;
            }
            for (int i = 0; i < 2; i++)
            {
                Instantiate(firewallPrefab, transform.position + Random.insideUnitSphere * 2f, Quaternion.identity);
            }
        }
        
        private void HandleProjectileHit(GameObject target)
        {
            if (target.gameObject.layer == LayerMask.NameToLayer("Player") /*Player.layer*/)
            {
                OnProjectileHitPlayer();
            }
        }

        public void OnProjectileHitPlayer()
        {
            Health = Mathf.Min(Health + 15f, config.backupStats.health);
        }
        
        private void OnDestroy()
        {
            foreach (Projectile proj in ProjectilePool.Instance.allProjectiles[projectileType])
            {
                if (proj.owner == this)
                {
                    proj.OnHit -= HandleProjectileHit;
                }
            }
        }
    }
}
