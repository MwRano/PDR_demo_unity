#nullable enable

using UnityEngine;
using TMPro;

/// <summary>
/// ユーザーの状態を管理するクラス
/// </summary>
public class UserManager : MonoBehaviour
{
    TMP_Text _userPositionText; // ユーザーの位置を表示するテキスト
    TMP_Text _userRotationText; // ユーザーの向きを表示するテキスト
    Transform _userTransform;


    public UserManager(Transform userTransform, TMP_Text positionText, TMP_Text rotationText)
    {
        _userTransform = userTransform;
        _userPositionText = positionText;
        _userRotationText = rotationText;
    }

    public void UpdateUserPosition(Vector3 position)
    {
        _userTransform.position = position;
        _userPositionText.text = $"User Position: {position.x:F2}, {position.y:F2}"; // 位置を表示
    }

    public void UpdateUserRotation(float cumulativeYaw)
    {
        _userTransform.rotation = Quaternion.Euler(0, 0, cumulativeYaw * Mathf.Rad2Deg - 90); // 回転量を計算
        _userRotationText.text = $"Heading: {cumulativeYaw * Mathf.Rad2Deg:F2}°"; // ラジアンを度に変換して表示
    }
}
