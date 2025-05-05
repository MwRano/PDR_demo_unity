#nullable enable

using UnityEngine;
using UnityEngine.UI;

public class DirectionInputHandler : MonoBehaviour
{
    public bool isDirectionSet{ get; set;} // ユーザーの向きが設定されたかどうか
    
    public float userDirectionYaw{ get; set;} // ユーザーの向き（ラジアン）

    UserManager _userManager; // ユーザーマネージャーの参照
    Button _userDirectionConfirmButton; // ボタンの参照
    Vector3 _userPosition;

    public void Initialize(UserManager userManager, Button userDirectionConfirmButton, Vector3 userPosition)
    {
        isDirectionSet = false; // 初期値を設定
        _userManager = userManager;
        _userDirectionConfirmButton = userDirectionConfirmButton;
        _userPosition = userPosition; // ユーザーの位置を設定
        _userDirectionConfirmButton.onClick.AddListener(OnConfirmButtonClicked); // ボタンがクリックされたときにUpdateInitialDirectionメソッドを呼び出す
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.mousePosition.y < 1780f)
        {
            UpdateInitialDirection();
        }
    }

    void UpdateInitialDirection()
    {
        Vector3 screenPosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        Vector3 userDirection = worldPosition - _userPosition; // 初期位置からのベクトルを計算
        userDirection.Normalize(); // 単位ベクトルに正規化

        userDirectionYaw = Mathf.Atan2(userDirection.y, userDirection.x);
        _userManager.UpdateUserDirection(userDirectionYaw); // ユーザーマネージャーに向きを更新

    }

    void OnConfirmButtonClicked()
    {
        // ボタンがクリックされたときの処理
        isDirectionSet = true; // ユーザーの向きが設定されたことを示すフラグを立てる
        _userDirectionConfirmButton.interactable = false; // ボタンを無効化
        this.enabled = false; // スクリプトを無効化して、Updateメソッドを停止
    }
}
