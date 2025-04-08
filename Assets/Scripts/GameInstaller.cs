using Enemies;
using UnityEngine;
using Zenject;

using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private ProjectilePool projectilePool;

    public override void InstallBindings()
    {
        Debug.Log("GameInstaller InstallBindings called");

        Container.BindInterfacesAndSelfTo<PlayerStats>()
            .AsSingle()
            .NonLazy();
        Debug.Log("PlayerStats bound");

        Container.Bind<UpgradeSelectionUI>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
        Debug.Log("UpgradeSelectionUI bound");

        Container.BindInterfacesAndSelfTo<UpgradeManager>()
            .AsSingle()
            .NonLazy();
        Debug.Log("UpgradeManager bound");

        if (projectilePool == null)
        {
            Debug.LogError("ProjectilePool is not assigned in GameInstaller!");
        }
        Container.Bind<ProjectilePool>()
            .FromInstance(projectilePool)
            .AsSingle()
            .NonLazy();
        Debug.Log("ProjectilePool bound");

        Container.Bind<BaseEnemy>()
            .FromComponentInHierarchy()
            .AsTransient();
        Debug.Log("BaseEnemy bound");
    }
}