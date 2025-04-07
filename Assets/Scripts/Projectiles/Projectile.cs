using Enemies;
using UnityEngine;
using Zenject;

namespace Projectiles
{
    public enum ProjectileType
    {
        Small,
        Large
    }
    public class Projectile : MonoBehaviour , IPausable
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifetime;
        [Inject] PlayerStats playerStats;
        private float damage;
        private float timeAlive;
        private Vector3 direction;
        public BaseEnemy owner;
        private bool isPaused;

        public delegate void ProjectileHitHandler(GameObject target);
        public event ProjectileHitHandler OnHit;
        public ProjectileType Type { get; private set; }

        public void Initialize(Vector3 dir, float dmg, ProjectileType type, BaseEnemy shooter)
        {
            direction = dir;
            damage = dmg;
            Type = type;
            owner = shooter;
            gameObject.SetActive(true);
        }

        public void Pause()
        {
            isPaused = true;
        }
         
       public void Resume()
        {
            
            isPaused = false;
        }

        private void Update()
        {
            if (isPaused) return;
            transform.position += direction * speed * Time.deltaTime;
            timeAlive += Time.deltaTime;
            if (timeAlive >= lifetime)
            {
                ProjectilePool.Instance.ReturnToPool(this);
                timeAlive = 0f;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") /*Player.layer*/)
            {
                playerStats.TakeDamage(damage);
                OnHit?.Invoke(other.gameObject);
                ProjectilePool.Instance.ReturnToPool(this);
            }
        }
        
        public void Deactivate()
        {
            gameObject.SetActive(false);
            owner = null;
        }
    }
}
