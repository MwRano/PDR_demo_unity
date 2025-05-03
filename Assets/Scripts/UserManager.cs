#nullable enable

using UnityEngine;
using TMPro;

/// <summary>
/// ユーザーの状態を管理するクラス
/// </summary>
public class UserManager : MonoBehaviour
{
    [Header("User Text")]
    [SerializeField] TMP_Text userPositionText; // ユーザーの位置を表示するテキスト
    [SerializeField] TMP_Text userRotationText; // ユーザーの向きを表示するテキスト
    Transform _userTransform;

    void Start()
    {
        _userTransform = gameObject.transform;
    }

    public void UpdateUserPosition(Vector3 position)
    {
        _userTransform.position = position;
        userPositionText.text = $"User Position: {position.x:F2}, {position.y:F2}"; // 位置を表示
    }

    public void UpdateUserRotation(float cumulativeYaw)
    {
        _userTransform.rotation = Quaternion.Euler(0, 0, cumulativeYaw * Mathf.Rad2Deg - 90); // 回転量を計算
        userRotationText.text = $"Heading: {cumulativeYaw * Mathf.Rad2Deg:F2}°"; // ラジアンを度に変換して表示
    }
}
