#nullable enable

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


    void OnAwake(){
        _userManager = new UserManager(userTransform, userPositionText, userRotationText);
        _floorLevelManager = new FloorLevelManager(floorLevelText, floorMaps); // フロアレベルマネージャーのインスタンスを作成
        _floorSelector = new FloorSelector(floorLevelDropdown, _floorLevelManager); // フロアセレクターのインスタンスを作成
        
    }

    void Update()
    {
        // 初期フロアの設定が完了したら、初期位置設定ハンドラーを起動
        if(_positionInputHandler is null && _floorSelector.isFloorLevelSet){
            _positionInputHandler = new PositionInputHandler(_userManager, userPositionConfirmButton); 
        }
        // 初期位置の設定が完了したら、初期向き設定ハンドラーを起動
        else if(_directionInputHandler is null && _positionInputHandler.isPositionSet){
            Vector3 userPosition = _positionInputHandler.userPostion; // ユーザーの位置を取得
            _directionInputHandler = new DirectionInputHandler(_userManager, userDirectionSetButton, userPosition);
        }
        // 初期向きの設定が完了したら、PDRマネージャーとフロアレベル推定器を起動
        else if(_pdrManager is null && _floorLevelEstimator is null && _directionInputHandler.isDirectionSet){
            float userDirectionYaw = _directionInputHandler.userDirectionYaw; // ユーザーの向きを取得
            _pdrManager = new PDRManager(_userManager, userDirectionYaw); // PDRマネージャーのインスタンスを作成

            int floorLevel = _floorSelector.selectedFloorLevel; // 選択されたフロアレベルを取得
            float floorLevelPressure = _floorSelector.selectedFloorLevelPressure; // 選択されたフロアレベルの気圧を取得
            _floorLevelEstimator = new FloorLevelEstimator(_floorLevelManager, floorLevel, floorLevelPressure); // フロアレベル推定器のインスタンスを作成
        }
    }
}