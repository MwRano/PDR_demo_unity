#nullable enable
using VContainer;
using VContainer.Unity;

// アプリの状態を管理する列挙型
public enum AppState
{
    FloorSelection,
    PositionInput,
    DirectionInput,
    PDRStarting
}

/// <summary>
/// アプリの状態管理をするクラス
/// </summary>
public class AppController : ITickable
{
    private FloorManager _floorManager;
    private AppPresenter _appPresenter;
    private PositionInputHandler _positionInputHandler;
    private DirectionInputHandler _directionInputHandler;
    private PDRManager _pdrManager;

    // 初期状態をフロア選択状態に設定
    private AppState _currentAppState { get; set; }

    [Inject]
    public AppController(
        FloorManager floorManager,
        AppPresenter appPresenter,
        PositionInputHandler positionInputHandler,
        DirectionInputHandler directionInputHandler,
        PDRManager pdrManager)
    {
        _floorManager = floorManager;
        _appPresenter = appPresenter;
        _positionInputHandler = positionInputHandler;
        _directionInputHandler = directionInputHandler;
        _pdrManager = pdrManager;

        // eventの登録
        _appPresenter.OnAppPhaseChanged += HandleAppPhaseChenged;

        _floorManager.DisplayFloorMap();
    }

    void ITickable.Tick()
    {
        // AppStateの状態管理
        switch (_currentAppState)
        {
            case AppState.PositionInput:
                _positionInputHandler.UpdateInitialPosition();
                break;
            case AppState.DirectionInput:
                _directionInputHandler.UpdateInitialDirection();
                break;
            case AppState.PDRStarting:
                _pdrManager.UpdateCumulativeYaw();
                if (_pdrManager.DetectStep()) _pdrManager.UpdatePosition();
                _floorManager.MonitorFloorChange();
                break;
        }
    }

    // Appのフェーズ移行時に実行されるメソッド
    void HandleAppPhaseChenged(AppState appState)
    {
        _currentAppState = appState;
    }
}