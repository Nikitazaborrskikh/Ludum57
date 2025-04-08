using Projectiles;
using UnityEngine;
using System.Collections;

namespace Enemies.SimpleEnemies
{
    public class Captcha : BaseEnemy
    {
        public AudioClip shootSound;
        public AudioClip DieSound;
        public AudioSource audioSource;
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
        
        private IEnumerator StartSound(AudioClip Sound)
        {
            audioSource.PlayOneShot(Sound);
            yield return null; 
        }
        public override void Attack(Vector3 playerPosition)
        {
            StartCoroutine(StartSound(shootSound));
            StartCoroutine(AttackSequence(playerPosition));
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
            }
            else rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
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