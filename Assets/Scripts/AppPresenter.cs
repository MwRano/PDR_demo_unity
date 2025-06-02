#nullable enable
using VContainer;
using R3;
using System;

/// <summary>
/// ViewとModelを仲介するクラス
/// </summary>
public class AppPresenter
{
    private AppViewMono _appViewMono;
    private FloorManager _floorManager;
    private PDRManager _pdrManager;
    private UserTrajectHandler _userTrajectHandler;
    private SceneLoader _sceneLoader;
    private UserMono _userMono;

    public event Action<AppState> OnAppPhaseChanged = null!;

    [Inject]
    public AppPresenter(
        AppViewMono appViewMono,
        FloorManager floorManager,
        PDRManager pdrManager,
        UserTrajectHandler userTrajectHandler,
        SceneLoader sceneLoader,
        UserMono userMono)
    {
        _appViewMono = appViewMono;
        _floorManager = floorManager;
        _pdrManager = pdrManager;
        _userTrajectHandler = userTrajectHandler;
        _sceneLoader = sceneLoader;
        _userMono = userMono;
        
        // eventの登録
        _appViewMono.OnFloorSelected += HandleFloorSelected;
        _appViewMono.OnUserPositionConfirmButtonClicked += HandleUserPositionDecided;
        _appViewMono.OnUserDirectionConfirmButtonClicked += HandleUserDirectionDecided;
        _appViewMono.OnUserTrajectoryToggleChanged += HandleTrajectoryToggleChanged;
        _appViewMono.OnMapMatchingToggleChanged += HandleMapMatchingToggleChanged;
        _appViewMono.OnReloadButtonClicked += HandleSceneReloaded;

        // subscribe
        _floorManager.CurrentFloorMapData
            .Skip(1)
            .Subscribe(mapData => _appViewMono.SetFloorLevelText(mapData.floorId))
            .AddTo(_appViewMono);

        _userMono.UserPosition
            .Skip(1)
            .Subscribe(position => _appViewMono.SetUserPositionText(position))
            .AddTo(_appViewMono);

        _userMono.UserComulativeYaw
            .Skip(1)
            .Subscribe(yaw => _appViewMono.SetUserDirectionText(yaw))
            .AddTo(_appViewMono);
    }
    
    // フロア選択時に実行されるメソッド
    private void HandleFloorSelected(int selectedIndex)
    {
        _floorManager.SetFloorLevel(selectedIndex);
        _appViewMono.SwitchToPositionSetup();
        OnAppPhaseChanged?.Invoke(AppState.PositionInput);
    }

    // 初期ユーザー位置決定時に実行されるメソッド
    private void HandleUserPositionDecided()
    {
        _appViewMono.SwitchToDirectionSetup();
        OnAppPhaseChanged?.Invoke(AppState.DirectionInput);
    }

    // 初期ユーザー向き決定時に実行されるメソッド
    private void HandleUserDirectionDecided()
    {
        _appViewMono.DeactivateInitialSetupUI();
        OnAppPhaseChanged?.Invoke(AppState.PDRStarting);
    }

    // 軌跡トグル切り替わり時に実行されるメソッド
    private void HandleTrajectoryToggleChanged(bool isTrajectOn)
    {
        _userTrajectHandler.OnToggleChanged(isTrajectOn);
    }

    // リロード時に実行されるメソッド
    private void HandleSceneReloaded()
    {
        _sceneLoader.ReloadScene();
    }

    // マップマッチングトグル切り替わり時に実行されるメソッド
    private void HandleMapMatchingToggleChanged(bool isMapMatchingOn)
    {
        _pdrManager.IsMapMatchingOn = isMapMatchingOn;
    }
}
