#nullable enable
using UnityEngine;
using VContainer;

/// <summary>
/// ユーザーの位置を指定する入力を扱うクラス
/// </summary>
public class PositionInputHandler
{
    public Vector3 UserPosition { get; set; } // ユーザーの位置
    private UserMono _userMono;

    Vector2 _touchStartPosition; // タッチ開始位置
    Vector2 _userStartPosition; // ユーザーの初期位置
    float _swipeSpeed = 0.01f; // スワイプの速度

    [Inject]
    public PositionInputHandler(UserMono userMono)
    {
        _userMono = userMono;
        UserPosition = new Vector3(0, 0, 0);
    }

    // ユーザーの初期位置を更新するメソッド
    public void UpdateInitialPosition()
    {
        // 画面入力範囲の設定
        if (Input.touchCount <= 0 || Input.mousePosition.y >= 1780f) return;

        if (Input.GetMouseButtonDown(0))
        {
            _touchStartPosition = Input.mousePosition; // タッチ開始位置を取得
            _userStartPosition = UserPosition; // ユーザーの初期位置を取得
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 touchCurrentPosition = Input.mousePosition; // タッチ現在位置を取得
            Vector2 touchDelta = touchCurrentPosition - _touchStartPosition; // タッチの移動量を計算
            Vector2 moveDirection = new Vector2(-touchDelta.x, -touchDelta.y) * _swipeSpeed; // 移動方向を計算

            UserPosition = _userStartPosition + moveDirection;
            _userMono.UpdateUserPosition(UserPosition);
        }
    }
}
