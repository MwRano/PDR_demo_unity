#nullable enable
using UnityEngine;
using VContainer;
using VContainer.Unity;

/// <summary>
/// アプリのスコープ
/// </summary>
public class AppScope : LifetimeScope
{
    [SerializeField] PDRParams pdrParam;
    [SerializeField] FloorEstimationParams floorEstimationParams;
    [SerializeField] FloorMapParams floorMapParams;

    protected override void Configure(IContainerBuilder builder)
    {
        // パラメータ
        builder.RegisterInstance(pdrParam).AsImplementedInterfaces();
        builder.RegisterInstance(floorEstimationParams).AsImplementedInterfaces();
        builder.RegisterInstance(floorMapParams).AsImplementedInterfaces();

        // app
        builder.RegisterEntryPoint<AppController>(Lifetime.Scoped);
        builder.Register<AppPresenter>(Lifetime.Scoped);
        builder.RegisterComponentInHierarchy<AppViewMono>();
        builder.RegisterComponentInHierarchy<UserMono>();
        builder.Register<PositionInputHandler>(Lifetime.Scoped);
        builder.Register<DirectionInputHandler>(Lifetime.Scoped);
        builder.Register<FloorManager>(Lifetime.Scoped);
        builder.Register<PDRManager>(Lifetime.Scoped);
        builder.Register<UserTrajectHandler>(Lifetime.Scoped);
        builder.Register<SceneLoader>(Lifetime.Scoped);
        builder.Register<MapMatching>(Lifetime.Scoped);
    }
}
