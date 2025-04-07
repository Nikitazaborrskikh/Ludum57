using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("GameInstaller InstallBindings called");
        Container.BindInterfacesAndSelfTo<PlayerStats>()
            .AsSingle()
            .NonLazy();
        Container.Bind<UpgradeManager>().AsSingle();
        Container.Bind<UpgradeSelectionUI>().FromComponentInHierarchy().AsSingle();
    }
}