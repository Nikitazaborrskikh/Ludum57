using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class ProjectilePool : MonoBehaviour
    {
        public static ProjectilePool Instance { get; private set; }

        [SerializeField] private GameObject smallProjectilePrefab;
        [SerializeField] private GameObject largeProjectilePrefab;
        [SerializeField] private int initialPoolSize = 20;
        [SerializeField] private int expansionSize = 10; // Количество объектов для расширения пула

        private Dictionary<ProjectileType, Queue<Projectile>> pools;
        public Dictionary<ProjectileType, List<Projectile>> allProjectiles; // Для отслеживания всех объектов

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializePool();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializePool()
        {
            pools = new Dictionary<ProjectileType, Queue<Projectile>>
            {
                { ProjectileType.Small, new Queue<Projectile>() },
                { ProjectileType.Large, new Queue<Projectile>() }
            };

            allProjectiles = new Dictionary<ProjectileType, List<Projectile>>
            {
                { ProjectileType.Small, new List<Projectile>() },
                { ProjectileType.Large, new List<Projectile>() }
            };

            // Создаем начальное количество объектов для каждого типа
            for (int i = 0; i < initialPoolSize; i++)
            {
                AddProjectileToPool(ProjectileType.Small, smallProjectilePrefab);
                AddProjectileToPool(ProjectileType.Large, largeProjectilePrefab);
            }
        }

        private void AddProjectileToPool(ProjectileType type, GameObject prefab)
        {
            GameObject projectileObj = Instantiate(prefab, transform);
            projectileObj.SetActive(false);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            pools[type].Enqueue(projectile);
            allProjectiles[type].Add(projectile); // Добавляем в список всех объектов
        }

        public Projectile GetProjectile(ProjectileType type, Vector3 position, Quaternion rotation)
        {
            Queue<Projectile> pool = pools[type];
            List<Projectile> allOfType = allProjectiles[type];

            // Проверяем, есть ли свободные объекты в пуле
            if (pool.Count == 0)
            {
                // Проверяем, все ли объекты активны
                bool allActive = true;
                foreach (Projectile proj in allOfType)
                {
                    if (!proj.gameObject.activeSelf)
                    {
                        allActive = false;
                        pool.Enqueue(proj); // Добавляем неактивный объект обратно в очередь
                        break;
                    }
                }

                // Если все объекты активны, расширяем пул
                if (allActive)
                {
                    for (int i = 0; i < expansionSize; i++)
                    {
                        GameObject prefab = type switch
                        {
                            ProjectileType.Small => smallProjectilePrefab,
                            ProjectileType.Large => largeProjectilePrefab,
                            _ => smallProjectilePrefab
                        };
                        AddProjectileToPool(type, prefab);
                    }

                    Debug.Log($"Expanded {type} projectile pool by {expansionSize}");
                }
            }

            Projectile projectile = pool.Dequeue();
            projectile.transform.position = position;
            projectile.transform.rotation = rotation;
            return projectile;
        }

        public void ReturnToPool(Projectile projectile)
        {
            projectile.Deactivate();
            pools[projectile.Type].Enqueue(projectile);
        }
    }
}