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

      

        Container.Bind<UpgradeSelectionUI>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy(); // Добавим NonLazy для явного создания
        Debug.Log("UpgradeSelectionUI bound");
        
        Container.BindInterfacesAndSelfTo<UpgradeManager>() // Указываем интерфейсы и сам класс
            .AsSingle()
            .NonLazy();
        Debug.Log("UpgradeManager bound");
    }
}