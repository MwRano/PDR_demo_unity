#nullable enable
using UnityEngine;
using VContainer;

/// <summary>
/// ユーザーの向きを指定する入力を扱うクラス
/// </summary>
public class DirectionInputHandler
{
    private UserMono _userMono;

    public float UserDirectionYaw { get; set; } // ユーザーの向き（ラジアン）

    [Inject]
    public DirectionInputHandler(
        UserMono userMono)
    {
        _userMono = userMono;
    }

    // ユーザーの初期方向を更新するメソッド
    public void UpdateInitialDirection()
    {
        //画面入力範囲の指定
        if (!Input.GetMouseButtonDown(0) || Input.mousePosition.y >= 1780f) return;

        Vector3 screenPosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        Vector3 userPosition = _userMono.gameObject.transform.position;
        Vector3 userDirection = worldPosition - userPosition;
        userDirection.Normalize();

        UserDirectionYaw = Mathf.Atan2(userDirection.y, userDirection.x);
        _userMono.UpdateUserDirection(UserDirectionYaw);
    }
}
