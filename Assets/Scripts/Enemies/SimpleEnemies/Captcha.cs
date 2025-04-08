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
                Projectile projectile = ProjectilePool.Instance.GetProjectile(
                    projectileType, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile>().Initialize(direction, DamagePerProjectile, projectileType, this);
                yield return new WaitForSeconds(0.2f); // Задержка 0.2 секунды между выстрелами
            }
        }

        public override void Move(Vector3 playerPosition)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            //transform.rotation = Quaternion.LookRotation(direction); //Добавить когда цх повернут на 90 по Y
            Quaternion lookRotation = Quaternion.LookRotation(direction); //Убрать когда цх повернут на 90 по Y
            transform.rotation = lookRotation * Quaternion.Euler(0, 90f, 0); //Убрать когда цх повернут на 90 по Y
            
            float distance = Vector3.Distance(transform.position, playerPosition);
            if (distance > DistanceToPlayer) // Дистанция атаки
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                transform.position = Vector3.MoveTowards(transform.position,
                    playerPosition,
                    MovementSpeed * Time.deltaTime);
            }
            else rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        
    }
}