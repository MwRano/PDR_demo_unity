#nullable enable

using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class AppController : MonoBehaviour
{
    // アプリの状態を管理する列挙型
    private enum AppState
    {
        FloorSelection,
        PositionInput,
        DirectionInput,
        PDRStarting
    }

    // 初期状態をフロア選択状態に設定
    private AppState _appState = AppState.FloorSelection;

    [Header("Parameters")]
    [SerializeField] PDRParams pdrParams; // PDRのパラメータ
    [SerializeField] FloorEstimationParams floorEstimationParams; // フロア推定のパラメータ
    [SerializeField] FloorMapParams floorMapParams; // フロアマップのパラメータ

    [Header("Floor Selector")]
    [SerializeField] TMP_Dropdown floorLevelDropdown; // フロアレベル選択用のドロップダウン

    [Header("Position Input Handler")]
    [SerializeField] Button userPositionConfirmButton; // ユーザー位置確認ボタン

    [Header("Direction Input Handler")]
    [SerializeField] Button userDirectionSetButton; // 初期向き設定ボタン

    [Header("User Manager")]
    [SerializeField] Transform userTransform; // UIマネージャーのプレハブ
    [SerializeField] TMP_Text userPositionText; // ユーザーの位置を表示するテキスト
    [SerializeField] TMP_Text userRotationText; // ユーザーの向きを表示するテキスト

    [Header("Floor Level Manager")]
    [SerializeField] TMP_Text floorLevelText; // フロアレベルを表示するテキスト

    [Header("User Trajectory")]
    [SerializeField] Toggle userTrajectoryToggle; // ユーザーの軌跡を表示するトグル

    FloorMapGenerator _floorMapGenerator; // フロアマップの生成クラス
    UserManager _userManager;
    FloorLevelManager _floorLevelManager;
    FloorSelector _floorSelector; // フロアセレクターのインスタンス
    PositionInputHandler _positionInputHandler; // 位置入力ハンドラーのインスタンス
    DirectionInputHandler _directionInputHandler; // 方向入力ハンドラーのインスタンス
    PDRManager _pdrManager; // PDRマネージャーのインスタンス
    FloorLevelEstimator _floorLevelEstimator; // フロアレベル推定器のインスタンス


    void Start()
    {
        _floorMapGenerator = new FloorMapGenerator(floorMapParams);
        GameObject floorMapParent = _floorMapGenerator.GenerateFloorMap();

        _userManager = gameObject.AddComponent<UserManager>();
        _userManager.Initialize(userTransform, userPositionText, userRotationText); // ユーザーマネージャーの初期化

        _floorLevelManager = gameObject.AddComponent<FloorLevelManager>();
        _floorLevelManager.Initialize(floorLevelText, floorMapParent); // フロアレベルマネージャーの初期化

        _floorSelector = new FloorSelector(floorLevelDropdown, _floorLevelManager);  

        userPositionConfirmButton.interactable = false; // 初期位置確認ボタンを無効化
        userDirectionSetButton.interactable = false; // 初期向き設定ボタンを無効化

        userTrajectoryToggle.gameObject.SetActive(false); // ユーザーの軌跡トグルを非表示

    }

    void Update()
    {
        switch (_appState)
        {
            case AppState.FloorSelection:
                OnFloorSelected();
                break;

            case AppState.PositionInput:
                OnPositionInput();
                break;

            case AppState.DirectionInput:
                OnDirectionInput();
                break;
        }
    }

    // フロアが選択されたときに実行するメソッド
    private void OnFloorSelected()
    {
        if (_floorSelector.isFloorLevelSet == false) return;

        userPositionConfirmButton.interactable = true;
        _positionInputHandler = gameObject.AddComponent<PositionInputHandler>();
        _positionInputHandler.Initialize(_userManager, userPositionConfirmButton);

        _appState = AppState.PositionInput;

    }

    // ユーザーの初期位置が確定されたときに実行するメソッド
    private void OnPositionInput()
    {
        if (_positionInputHandler is null || _positionInputHandler.isPositionSet == false) return;

        userDirectionSetButton.interactable = true;
        Vector3 userPosition = _positionInputHandler.userPosition;
        _directionInputHandler = gameObject.AddComponent<DirectionInputHandler>();
        _directionInputHandler.Initialize(_userManager, userDirectionSetButton, userPosition);

        _appState = AppState.DirectionInput;

    }

    // ユーザーの初期方向が確定されたときに実行するメソッド
    private void OnDirectionInput()
    {
        if (_directionInputHandler is null || _directionInputHandler.isDirectionSet == false) return;

        floorLevelDropdown.interactable = false;
        userTrajectoryToggle.gameObject.SetActive(true);

        float userDirectionYaw = _directionInputHandler.userDirectionYaw;
        Vector3 userPosition = _positionInputHandler.userPosition;

        _pdrManager = gameObject.AddComponent<PDRManager>();
        _pdrManager.Initialize(_userManager, userDirectionYaw, userPosition, pdrParams);

        int floorLevel = _floorSelector.selectedFloorLevel;
        float floorLevelPressure = _floorSelector.selectedFloorLevelPressure;

        _floorLevelEstimator = gameObject.AddComponent<FloorLevelEstimator>();
        _floorLevelEstimator.Initialize(_floorLevelManager, floorLevel, floorLevelPressure, floorEstimationParams);

        _appState = AppState.PDRStarting;

    }


    
    
}