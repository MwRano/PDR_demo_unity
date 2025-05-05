using UnityEngine;
using TMPro; // TextMeshProを使用するための名前空間
using UnityEngine.UI;

public class PositionInputHandler : MonoBehaviour
{
    public Vector3 userPosition{ get; set; } // ユーザーの位置
    public bool isPositionSet{ get; set;} // ユーザーの位置が設定されたかどうか

    UserManager _userManager; // ユーザーマネージャーの参照
    Button _userPositionConfirmButton; // ボタンの参照

    Vector2 _touchStartPosition; // タッチ開始位置
    Vector2 _userStartPosition; // ユーザーの初期位置
    float _swipeSpeed = 0.001f; // スワイプの速度

    public void Initialize(UserManager userManager, Button userPositionConfirmButton)
    {
        isPositionSet = false; // 初期値を設定
        _userManager = userManager;
        _userPositionConfirmButton = userPositionConfirmButton;
        _userPositionConfirmButton.onClick.AddListener(OnConfirmButtonClicked); // ボタンがクリックされたときにUpdateInitialPositionメソッドを呼び出す
    }

    void Update()
    {
        // タッチが検出された場合、初期位置を更新
        if (Input.touchCount > 0 && Input.mousePosition.y < 1780f)
        {
            UpdateInitialPosition();
        }
    }

    // void UpdateInitialPosition()
    // {
    //     Vector3 screenPosition = Input.mousePosition;
    //     Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
    //     worldPosition.z = 0;



    //     userPostion = worldPosition; // ユーザーの位置を更新
    //     _userManager.UpdateUserPosition(userPostion); // ユーザーマネージャーに位置を更新
    // }

    void UpdateInitialPosition()
    {

        if (Input.GetMouseButtonDown(0)){
            _touchStartPosition = Input.mousePosition; // タッチ開始位置を取得
            _userStartPosition = userPosition; // ユーザーの初期位置を取得
        }

        if(Input.GetMouseButton(0)){
            Vector2 touchCurrentPosition = Input.mousePosition; // タッチ現在位置を取得
            Vector2 touchDelta = touchCurrentPosition - _touchStartPosition; // タッチの移動量を計算
            Vector2 moveDirection = new Vector2(-touchDelta.x, -touchDelta.y) * _swipeSpeed; // 移動方向を計算

            userPosition = _userStartPosition + moveDirection; // ユーザーの位置を更新  
            _userManager.UpdateUserPosition(userPosition);

        }

    }


    void OnConfirmButtonClicked()
    {
        // ボタンがクリックされたときの処理
        isPositionSet = true; // ユーザーの位置が設定されたことを示すフラグを立てる
        _userPositionConfirmButton.interactable = false; // ボタンを無効化
        this.enabled = false; // スクリプトを無効化して、Updateメソッドを停止
    }
}
