#nullable enable

using UnityEngine;

/// <summary>
/// ユーザーの状態を管理するクラス
/// </summary>
public class UserManager : MonoBehaviour
{
    Transform _userTransform;

    void OnAwake()
    {
        _userTransform = gameObject.transform;
    }

    void UpdateUserPosition(Vector3 position)
    {
        _userTransform.position = position; 
    }

    void UpdateUserRotation(float cumulativeYaw)
    {
        _userTransform.rotation = Quaternion.Euler(0, 0, cumulativeYaw * Mathf.Rad2Deg - 90); // 回転量を計算
    }
}
