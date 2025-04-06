using UnityEngine;

namespace Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IEnemy
    {
        public float Health { get; set; }
        public abstract float AttackSpeed { get; }
        public abstract float DamagePerProjectile { get; }
        public abstract float MovementSpeed { get; }

        protected float attackTimer;
        protected GameObject projectilePrefab;
        
        [SerializeField] private GameObject player;

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

        protected Vector3 FindPlayerPosition()
        {
            return player.transform.position;
        }
    }
}
