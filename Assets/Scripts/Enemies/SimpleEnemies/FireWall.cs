using Projectiles;
using UnityEngine;
using Zenject;

namespace Enemies.SimpleEnemies
{
    public class Firewall : BaseEnemy
    {
        public override float AttackSpeed => config.firewallStats.attackSpeed;
        public override float DamagePerProjectile => config.firewallStats.damagePerProjectile;
        public override float MovementSpeed => /*PlayerStats.Speed*/ 5f / config.firewallStats.movementSpeedDivider;
        public override float DistanceToPlayer => config.firewallStats.distanceToPlayer;

        private ProjectileType projectileType => config.firewallStats.projectileType;
        private Rigidbody rb;
        private Animator animator;
        
        [Inject] private UpgradeManager upgradeManager;

        private void Awake()
        {
            Health = config.firewallStats.health;
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        public override void Attack(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            for (int i = -1; i <= 1; i++)
            {
                Vector3 spread = Quaternion.Euler(0, 15 * i, 0) * direction;
                Quaternion rotation = Quaternion.LookRotation(spread);
                Projectile projectile = ProjectilePool.Instance.GetProjectile(
                    GetProjectilePrefab(),
                    projectileType,
                    transform.position,
                    rotation
                );
                projectile.Initialize(GetProjectilePrefab(), spread, rotation, DamagePerProjectile, projectileType, this);
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

        protected override GameObject GetProjectilePrefab()
        {
            return config.firewallStats.projectilePrefab;
        }

        protected override ProjectileType GetProjectileType()
        {
            return config.firewallStats.projectileType;
        }
    }
}