using System;
using Projectiles;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IEnemy, IPausable
    {
        [SerializeField] protected EnemyConfig config;
        [Inject] protected UpgradeManager upgradeManager;
        [Inject] protected ProjectilePool projectilePool;

        public float Health { get; set; }
        public abstract float AttackSpeed { get; }
        public abstract float DamagePerProjectile { get; }
        public abstract float MovementSpeed { get; }
        public abstract float DistanceToPlayer { get; }
        public event Action OnEnemyDiedEvent;

        private GameObject player;
        private float attackTimer;
        private bool isPaused;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        protected virtual void Update()
        {
            if (isPaused) return;
            attackTimer += Time.deltaTime;
            Vector3 playerPos = FindPlayerPosition();
            Move(playerPos);

            if (attackTimer >= AttackSpeed)
            {
                Attack(playerPos);
                attackTimer = 0f;
            }
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        public virtual void TakeDamage(float damage)
        {
            Debug.Log($"Enemy {gameObject.name} took {damage} damage");
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        public virtual void Attack(Vector3 playerPosition)
        {
            // Базовая реализация, может быть переопределена
            Vector3 direction = (playerPosition - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Projectile projectile = projectilePool.GetProjectile(
                GetProjectilePrefab(),
                GetProjectileType(), // Добавлен тип
                transform.position,
                rotation
            );
            projectile.Initialize(GetProjectilePrefab(), direction, rotation, DamagePerProjectile, GetProjectileType(), this);
        }

        public abstract void Move(Vector3 playerPosition);

        protected virtual GameObject GetProjectilePrefab()
        {
            return config.captchaStats.projectilePrefab; // Базовая реализация, переопределяется в наследниках
        }

        protected virtual ProjectileType GetProjectileType()
        {
            return config.captchaStats.projectileType; // Базовая реализация
        }

        private Vector3 FindPlayerPosition()
        {
            return player.transform.position;
        }

        public virtual void Die()
        {
            OnEnemyDiedEvent?.Invoke();
            if (Random.value < 0.3f)
            {
                upgradeManager.OfferUpgrades();
            }
            Destroy(gameObject);
        }
    }
}