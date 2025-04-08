using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Projectiles;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    [SerializeField] private int initialPoolSize = 20;
    [SerializeField] private int expansionSize = 10;

    [Inject] private DiContainer container; // Для инъекции зависимостей

    private Dictionary<GameObject, Queue<Projectile>> pools; // Пул по префабам
    public Dictionary<GameObject, List<Projectile>> allProjectiles; // Все снаряды по префабам

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
        pools = new Dictionary<GameObject, Queue<Projectile>>();
        allProjectiles = new Dictionary<GameObject, List<Projectile>>();
    }

    private void AddProjectileToPool(GameObject prefab, ProjectileType type)
    {
        GameObject projectileObj = container.InstantiatePrefab(prefab, transform); // Создаём через Zenject
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        container.Inject(projectile); // Инжектируем зависимости (PlayerStats)
        projectile.Deactivate();
        pools[prefab].Enqueue(projectile);
        allProjectiles[prefab].Add(projectile);
    }

    public Projectile GetProjectile(GameObject prefab, ProjectileType type, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new Queue<Projectile>();
            allProjectiles[prefab] = new List<Projectile>();
            // Инициализируем пул для нового префаба
            for (int i = 0; i < initialPoolSize; i++)
            {
                AddProjectileToPool(prefab, type);
            }
        }

        Queue<Projectile> pool = pools[prefab];
        List<Projectile> allOfType = allProjectiles[prefab];

        if (pool.Count == 0)
        {
            bool allActive = true;
            foreach (Projectile proj in allOfType)
            {
                if (!proj.gameObject.activeSelf)
                {
                    allActive = false;
                    pool.Enqueue(proj);
                    break;
                }
            }

            if (allActive)
            {
                for (int i = 0; i < expansionSize; i++)
                {
                    AddProjectileToPool(prefab, type);
                }
                Debug.Log($"Expanded pool for prefab {prefab.name} by {expansionSize}");
            }
        }

        Projectile projectile = pool.Dequeue();
        projectile.transform.position = position;
        projectile.transform.rotation = rotation * Quaternion.Euler(0, 90f, 0);
        return projectile;
    }

    public void ReturnToPool(Projectile projectile)
    {
        projectile.Deactivate();
        pools[projectile.Prefab].Enqueue(projectile); // Используем свойство Prefab напрямую
    }
}