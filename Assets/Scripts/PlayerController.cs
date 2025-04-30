using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 _playerPosition;
    public Vector3 PlayerPosition
    {
        get => _playerPosition;
        set
        {
            _playerPosition = value;
            transform.position = _playerPosition; // GameObject の位置を更新
        }
    }

    private Vector3 _playerHeading;
    public Vector3 PlayerHeading
    {
        get => _playerHeading;
        set
        {
            _playerHeading = value;
            float playerYaw = Mathf.Atan2(_playerHeading.y, _playerHeading.x); // 初期向きを設定
            gameObject.transform.rotation = Quaternion.Euler(0, 0, playerYaw * Mathf.Rad2Deg - 90); // 初期向きを反映
        }
    }

}
