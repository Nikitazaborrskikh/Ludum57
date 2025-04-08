using System.Collections;
using Projectiles;
using UnityEngine;

namespace Enemies.Bosses
{
    public class Backup : BaseEnemy
    {
        public AudioClip shootSound;
        public AudioClip DieSound;
        public GameObject audioSource;
        public override float AttackSpeed => config.backupStats.attackSpeed;
        public override float DamagePerProjectile => config.backupStats.damagePerProjectile;
        public override float MovementSpeed => /*PlayerStats.Speed*/ 5f / config.backupStats.movementSpeedDivider;
        public override float DistanceToPlayer => config.backupStats.distanceToPlayer;

        private ProjectileType projectileType => config.backupStats.projectileType;
        private Rigidbody rb;
        private Animator animator;
        
        private float spawnTimer;
        public GameObject firewallPrefab;

        private void Awake()
        {
            Health = config.backupStats.health;
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            StartCoroutine(SpawnFirewallRoutine());
        }

        private IEnumerator StartSound(AudioClip Sound)
        {
            audioSource.GetComponent<AudioSource>().PlayOneShot(Sound);
            yield return null;
        }

        public override void Attack(Vector3 playerPosition)
        {
            StartCoroutine(StartSound(shootSound));
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

        public override void TakeDamage(float damage)
        {
            Debug.Log($"Enemy {gameObject.name} took {damage} damage");
            Health -= damage;
            if (Health <= 0)
            {
                StartCoroutine(StartSound(DieSound));
                Die();
            }
        }

        public override void Move(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            //transform.rotation = Quaternion.LookRotation(direction); //Добавить когда цх повернут на 90 по Y
            Quaternion lookRotation = Quaternion.LookRotation(direction); //Убрать когда цх повернут на 90 по Y
            transform.rotation = lookRotation * Quaternion.Euler(0, 90f, 0); //Убрать когда цх повернут на 90 по Y
            
            float distance = Vector3.Distance(transform.position, playerPosition);
            if (distance > DistanceToPlayer)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                transform.position = Vector3.MoveTowards(transform.position,
                    playerPosition,
                    MovementSpeed * Time.deltaTime);
                animator.SetBool("isMoving", true);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                animator.SetBool("isMoving", false);
            }
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