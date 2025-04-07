using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] protected EnemyConfig config;
        [Inject] protected UpgradeManager upgradeManager;
        public float Health { get; set; }
        public abstract float AttackSpeed { get; }
        public abstract float DamagePerProjectile { get; }
        public abstract float MovementSpeed { get; }
        public abstract float DistanceToPlayer { get; }
        
        private GameObject player;
        private float attackTimer;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

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
            Debug.Log("Aй бляяяяять");
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        public abstract void Attack(Vector3 playerPosition);
        public abstract void Move(Vector3 playerPosition);

        private Vector3 FindPlayerPosition()
        {
            return player.transform.position;
        }

        public virtual void Die()
        {
          //  if (Random.value < 0.3f) // 30% шанс
           // {
                upgradeManager.OfferUpgrades();
           // }
            Destroy(gameObject);
        }
    }
}
