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
        private Rigidbody rb;

        private float spawnTimer;
        public GameObject firewallPrefab;

        private void Awake()
        {
            Health = config.backupStats.health;
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            StartCoroutine(SpawnFirewallRoutine());
        }

        public override void Attack(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            float angleStep = 15f;
            float startAngle = -angleStep * 2.5f;

            for (int i = 0; i < 6; i++)
            {
                float angle = startAngle + (i * angleStep);
                Vector3 spread = Quaternion.Euler(0, angle, 0) * direction;
                Quaternion rotation = Quaternion.LookRotation(spread);
                Projectile projectile = ProjectilePool.Instance.GetProjectile(
                    GetProjectilePrefab(),
                    projectileType,
                    transform.position,
                    rotation
                );
                projectile.Initialize(GetProjectilePrefab(), spread, rotation, DamagePerProjectile, projectileType, this);
                projectile.OnHit += HandleProjectileHit;
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

        private IEnumerator SpawnFirewallRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(10f, 15f));
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
            if (target.gameObject.layer == LayerMask.NameToLayer("Player"))
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
            foreach (var projectileList in ProjectilePool.Instance.allProjectiles.Values)
            {
                foreach (Projectile proj in projectileList)
                {
                    if (proj.owner == this)
                    {
                        proj.OnHit -= HandleProjectileHit;
                    }
                }
            }
        }

        protected override GameObject GetProjectilePrefab()
        {
            return config.backupStats.projectilePrefab;
        }

        protected override ProjectileType GetProjectileType()
        {
            return config.backupStats.projectileType;
        }
    }
}