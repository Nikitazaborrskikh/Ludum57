using Projectiles;
using UnityEngine;

namespace Enemies.Bosses
{
    public class TwoFactorAuth : BaseEnemy
    {
        public override float AttackSpeed => currentPhase == 1 ? 
            config.twoFactorAuth.phase1.attackSpeed : 
            config.twoFactorAuth.phase2.attackSpeed;
        public override float DamagePerProjectile => currentPhase == 1 ? 
            config.twoFactorAuth.phase1.damagePerProjectile : 
            config.twoFactorAuth.phase2.damagePerProjectile;
        public override float MovementSpeed => /*PlayerStats.Speed*/ 5f / (currentPhase == 1 ? 
            config.twoFactorAuth.phase1.movementSpeedDivider : 
            config.twoFactorAuth.phase2.movementSpeedDivider);
        public override float DistanceToPlayer => currentPhase == 1 ? 
            config.twoFactorAuth.phase1.distanceToPlayer : 
            config.twoFactorAuth.phase2.distanceToPlayer;
        
        private ProjectileType projectileType => config.twoFactorAuth.phase1.projectileType;
        private Rigidbody rb;

        private int currentPhase = 1;

        private void Awake()
        {
            Health = config.twoFactorAuth.phase1.health;
            rb = GetComponent<Rigidbody>();
        }

        public override void Attack(Vector3 playerPosition)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3 direction = Quaternion.Euler(0, 60 * i, 0) * Vector3.forward;
                Projectile projectile = ProjectilePool.Instance.GetProjectile(
                    projectileType, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile>().Initialize(direction, DamagePerProjectile, projectileType, this);
            }
        }

        public override void Move(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            
            float distance = Vector3.Distance(transform.position, playerPosition);
            if (distance > DistanceToPlayer) // Дистанция атаки
            {
                rb.constraints = RigidbodyConstraints.None;
                transform.position = Vector3.MoveTowards(transform.position,
                    playerPosition,
                    MovementSpeed * Time.deltaTime);
            }
            else rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        public override void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0 && currentPhase == 1)
            {
                currentPhase = 2;
                Health = config.twoFactorAuth.phase2.health;
            }
            else if (Health <= 0 && currentPhase == 2)
            {
                Die();
                Destroy(gameObject);
            }
        }
    }
}
