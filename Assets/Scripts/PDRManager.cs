# nullable enable

using UnityEngine;
using UnityEngine.Rendering;


public class PDRManager : MonoBehaviour
{
    float stepLength = 0.1f; // ステップの長さ
    float stepThreshold = 0.2f; // ステップ検出の閾値（加速度の変化量）
    float rotationSpeedFactor = 1.0f; // ジャイロの回転速度にかける係数

    UserManager _userManager; // ユーザーマネージャーの参照
    float _cumulativeYaw; // Z軸回りの累積回転角度
    Vector3 _lastAcceleration;
    Vector3 _userPosition;

    public PDRManager(UserManager userManager, float userDirectionYaw)
    {
        _userManager = userManager;
        _cumulativeYaw = userDirectionYaw; // 初期向きを設定
        _lastAcceleration = Input.acceleration;
    }

    void Update()
    {
        UpdateCumulativeYaw();

        if(DetectStep())
        {
            UpdatePosition();
        }
    }

    bool DetectStep()
    {
        bool isStepping = false;
        float accelerationChange = Mathf.Abs(Input.acceleration.magnitude - _lastAcceleration.magnitude);
        if (accelerationChange > stepThreshold && !isStepping)
        {
            isStepping = true;

        }
        else if (accelerationChange < stepThreshold * 0.5f) // 閾値を下回ったらステップ終了とみなす
        {
            isStepping = false;
        }

        _lastAcceleration = Input.acceleration;

        return isStepping;
    }

    void UpdatePosition()
    {
        Vector3 forward = new Vector3(Mathf.Cos(_cumulativeYaw), Mathf.Sin(_cumulativeYaw), 0).normalized;
        _userPosition += forward * stepLength;
        _userManager.UpdateUserPosition(_userPosition); // ユーザーマネージャーに位置を更新
    }

    void UpdateCumulativeYaw()
    {
        _cumulativeYaw += Input.gyro.rotationRate.z * Time.deltaTime * rotationSpeedFactor;
        _userManager.UpdateUserDirection(_cumulativeYaw); // ユーザーマネージャーに回転を更新
    }
}
