using Projectiles;
using UnityEngine;
using System.Collections;

namespace Enemies.SimpleEnemies
{
    public class UserPage : BaseEnemy
    {
        public AudioClip shootSound;
        public AudioClip DieSound;
        public GameObject audioSource;
        public override float AttackSpeed => config.userPageStats.attackSpeed;
        public override float DamagePerProjectile => config.userPageStats.damagePerProjectile;
        public override float MovementSpeed => /*PlayerStats.Speed*/ 5f / config.userPageStats.movementSpeedDivider;
        public override float DistanceToPlayer => config.userPageStats.distanceToPlayer;

        private ProjectileType projectileType => config.userPageStats.projectileType;
        private Rigidbody rb;
        private Animator animator;
        private void Awake()
        {
            Health = config.userPageStats.health;
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
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
            transform.rotation = Quaternion.LookRotation(direction);

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