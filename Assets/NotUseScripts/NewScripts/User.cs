using UnityEngine;

/// <summary>
/// userを管理するクラス
/// </summary>
public class User : MonoBehaviour
{
    [SerializeField] PDRManager pdrManager;
    private Transform _userTransform;

    void OnAwake()
    {
        _userTransform = gameObject.transform;
        pdrManager.OnPositionUpdated += UpdateUserPosition;
        pdrManager.OnRotationUpdated += UpdateUserRotation;
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
