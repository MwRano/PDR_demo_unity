# nullable enable
using UnityEngine;

/// <summary>
/// userを管理するクラス
/// </summary>
public class User : MonoBehaviour
{
    private Transform _userTransform;

    void OnAwake()
    {
        _userTransform = gameObject.transform;
    }

    void Move(Vector3 deltaPosition)
    {
        _userTransform.position += deltaPosition; // 移動量を加算
        Debug.Log("Move");
    }

    void Rotate(Quaternion deltaRotation)
    {
        _userTransform.rotation *= deltaRotation; // ユーザーの回転を更新
        Debug.Log("Rotate");
    }

}
