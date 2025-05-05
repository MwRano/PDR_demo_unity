using UnityEngine;
using TMPro; // TextMeshProを使用するための名前空間
using UnityEngine.UI;

public class PositionInputHandler : MonoBehaviour
{
    public Vector3 userPostion{ get; set; } // ユーザーの位置
    public bool isPositionSet{ get; set;} // ユーザーの位置が設定されたかどうか

    UserManager _userManager; // ユーザーマネージャーの参照
    Button _userPositionConfirmButton; // ボタンの参照
 
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

    void UpdateInitialPosition()
    {
        // タップしたスクリーン座標を取得
        Vector3 screenPosition = Input.mousePosition;

        // スクリーン座標をワールド座標に変換
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        userPostion = worldPosition; // ユーザーの位置を更新
        _userManager.UpdateUserPosition(userPostion); // ユーザーマネージャーに位置を更新
    }

    void OnConfirmButtonClicked()
    {
        // ボタンがクリックされたときの処理
        isPositionSet = true; // ユーザーの位置が設定されたことを示すフラグを立てる
        _userPositionConfirmButton.interactable = false; // ボタンを無効化
        this.enabled = false; // スクリプトを無効化して、Updateメソッドを停止
    }
}
