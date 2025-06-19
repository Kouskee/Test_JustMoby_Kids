using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Cube _cubePrefab;

    public override void InstallBindings()
    {
        // Services
        Container.Bind<IGameService>().To<GameService>().AsSingle();
        Container.Bind<IConfigService>().To<ConfigService>().AsSingle();
        Container.Bind<ISaveService>().To<PlayerPrefsSaveService>().AsSingle();
        Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
        Container.Bind<INotificationService>().To<NotificationService>().AsSingle();
        Container.Bind<ITowerService>().To<TowerService>().AsSingle();
        Container.Bind<IScrollService>().To<ScrollService>().AsSingle();
        Container.Bind<IDragService>().To<DragService>().AsSingle();
        Container.Bind<IHoleService>().To<HoleService>().AsSingle();
        Container.Bind<IAnimationService>().To<DOTweenAnimationService>().AsSingle();
        Container.Bind<ICommandService>().To<CommandService>().AsSingle();

        // Config
        Container.Bind<GameConfig>().FromInstance(_gameConfig);

        // UI
        Container.Bind<GameController>().FromComponentInHierarchy().AsSingle();

        // Factory
        Container.BindFactory<Cube, PlaceholderFactory<Cube>>()
            .FromComponentInNewPrefab(_cubePrefab)
            .WithGameObjectName("Cube");
    }
}