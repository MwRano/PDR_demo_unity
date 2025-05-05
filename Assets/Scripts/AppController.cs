

using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class AppController : MonoBehaviour
{

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
    [SerializeField] GameObject floorMaps; // フロアマップの親オブジェクト

    UserManager _userManager; 
    FloorLevelManager _floorLevelManager;
    FloorSelector _floorSelector; // フロアセレクターのインスタンス
    PositionInputHandler _positionInputHandler; // 位置入力ハンドラーのインスタンス
    DirectionInputHandler _directionInputHandler; // 方向入力ハンドラーのインスタンス
    PDRManager _pdrManager; // PDRマネージャーのインスタンス
    FloorLevelEstimator _floorLevelEstimator; // フロアレベル推定器のインスタンス


    void Start()
    {
        _userManager = gameObject.AddComponent<UserManager>();
        _userManager.Initialize(userTransform, userPositionText, userRotationText); // ユーザーマネージャーの初期化

        _floorLevelManager = gameObject.AddComponent<FloorLevelManager>();
        _floorLevelManager.Initialize(floorLevelText, floorMaps); // フロアレベルマネージャーの初期化

        _floorSelector = gameObject.AddComponent<FloorSelector>();
        _floorSelector.Initialize(floorLevelDropdown, _floorLevelManager); // フロアセレクターの初期化
        
    }

    void Update()
    {
        // 初期フロアの設定が完了したら、初期位置設定ハンドラーを起動
        if(_positionInputHandler is null && _floorSelector.isFloorLevelSet){
            _positionInputHandler = gameObject.AddComponent<PositionInputHandler>(); // 位置入力ハンドラーのインスタンスを作成
            _positionInputHandler.Initialize(_userManager, userPositionConfirmButton); // 位置入力ハンドラーの初期化
        }
        // 初期位置の設定が完了したら、初期向き設定ハンドラーを起動
        else if(_directionInputHandler is null && _positionInputHandler is not null &&_positionInputHandler.isPositionSet){
            Vector3 userPosition = _positionInputHandler.userPosition; // ユーザーの位置を取得
            _directionInputHandler = gameObject.AddComponent<DirectionInputHandler>(); // 方向入力ハンドラーのインスタンスを作成
            _directionInputHandler.Initialize(_userManager, userDirectionSetButton, userPosition); // 方向入力ハンドラーの初期化
        }
        // 初期向きの設定が完了したら、PDRマネージャーとフロアレベル推定器を起動
        else if(_pdrManager is null && _floorLevelEstimator is null && _directionInputHandler is not null && _directionInputHandler.isDirectionSet){
            float userDirectionYaw = _directionInputHandler.userDirectionYaw; // ユーザーの向きを取得
            _pdrManager = gameObject.AddComponent<PDRManager>(); // PDRマネージャーのインスタンスを作成
            _pdrManager.Initialize(_userManager, userDirectionYaw); // PDRマネージャーの初期化

            int floorLevel = _floorSelector.selectedFloorLevel; // 選択されたフロアレベルを取得
            float floorLevelPressure = _floorSelector.selectedFloorLevelPressure; // 選択されたフロアレベルの気圧を取得
            _floorLevelEstimator = gameObject.AddComponent<FloorLevelEstimator>(); // フロアレベル推定器のインスタンスを作成
            _floorLevelEstimator.Initialize(_floorLevelManager, floorLevel, floorLevelPressure); // フロアレベル推定器の初期化
        }
    }
}