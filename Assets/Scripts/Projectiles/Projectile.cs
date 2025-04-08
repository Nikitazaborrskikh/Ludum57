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

    public class Projectile : MonoBehaviour, IPausable
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifetime;
        [Inject] private PlayerStats playerStats;
        private float damage;
        private float timeAlive;
        private Vector3 direction;
        public BaseEnemy owner;
        private bool isPaused;
        private GameObject prefab; // Ссылка на префаб для возврата в правильный пул

        public delegate void ProjectileHitHandler(GameObject target);
        public event ProjectileHitHandler OnHit;
        public ProjectileType Type { get; private set; }
        public GameObject Prefab => prefab;

        private void Awake()
        {
            if (playerStats == null)
            {
                Debug.LogError($"playerStats is null in Projectile Awake on {gameObject.name}");
            }
            else
            {
                Debug.Log($"playerStats injected successfully in Projectile Awake on {gameObject.name}");
            }
        }

        public void Initialize(GameObject prefab, Vector3 dir, Quaternion rotation, float dmg, ProjectileType type, BaseEnemy shooter)
        {
            this.prefab = prefab;
            Type = type;
            direction = dir;
            transform.rotation = rotation * Quaternion.Euler(0, 90f, 0);
            damage = dmg;
            owner = shooter;
            gameObject.SetActive(true);
            timeAlive = 0f;
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
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (playerStats == null)
                {
                    Debug.LogError($"playerStats is null in OnTriggerEnter on {gameObject.name}");
                    return;
                }
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