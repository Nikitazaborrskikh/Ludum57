using UnityEngine;
using Zenject;

namespace Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] protected EnemyConfig config;
        [SerializeField] private GameObject player;
        
        public float Health { get; set; }
        public abstract float AttackSpeed { get; }
        public abstract float DamagePerProjectile { get; }
        public abstract float MovementSpeed { get; }
        public abstract float DistanceToPlayer { get; }
        
        private float attackTimer;

        protected virtual void Update()
        {
            attackTimer += Time.deltaTime;
            Vector3 playerPos = FindPlayerPosition();
            Move(playerPos);
        
            if (attackTimer >= AttackSpeed)
            {
                Attack(playerPos);
                attackTimer = 0f;
            }
        }

        public virtual void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public abstract void Attack(Vector3 playerPosition);
        public abstract void Move(Vector3 playerPosition);

        private Vector3 FindPlayerPosition()
        {
            return player.transform.position;
        }
    }
}
