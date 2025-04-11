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
        [SerializeField] private float triggerRadius;
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
        private bool isPlayerInTriggerZone; // Флаг, показывающий, находится ли игрок в зоне

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            SetupTriggerZone(); // Настраиваем триггер-зону
        }

        protected virtual void Update()
        {
            if (isPaused || !isPlayerInTriggerZone) return; // Ничего не делаем, если пауза или игрок вне зоны

            attackTimer += Time.deltaTime;
            Vector3 playerPos = FindPlayerPosition();
            Move(playerPos);

            if (attackTimer >= AttackSpeed)
            {
                Attack(playerPos);
                attackTimer = 0f;
            }
        }
        
        private void SetupTriggerZone()
        {
            // Добавляем или настраиваем триггер-коллайдер
            SphereCollider triggerCollider = gameObject.GetComponent<SphereCollider>();
            if (triggerCollider == null)
            {
                triggerCollider = gameObject.AddComponent<SphereCollider>();
            }
            triggerCollider.isTrigger = true; // Устанавливаем как триггер
            triggerCollider.radius = triggerRadius; // Устанавливаем радиус
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInTriggerZone = true; // Игрок вошёл в зону
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
            Vector3 direction = (playerPosition - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Projectile projectile = projectilePool.GetProjectile(
                GetProjectilePrefab(),
                GetProjectileType(),
                transform.position,
                rotation
            );
            projectile.Initialize(GetProjectilePrefab(), direction, rotation, DamagePerProjectile, GetProjectileType(), this);
        }

        public abstract void Move(Vector3 playerPosition);

        protected virtual GameObject GetProjectilePrefab()
        {
            return config.captchaStats.projectilePrefab;
        }

        protected virtual ProjectileType GetProjectileType()
        {
            return config.captchaStats.projectileType;
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