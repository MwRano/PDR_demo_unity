# nullable enable

using UnityEngine;
using UnityEngine.Rendering;


public class PDRManager : MonoBehaviour
{
    float _stepLength; // ステップの長さ
    float _stepThreshold; // ステップ検出の閾値（加速度の変化量）
    float _rotationSpeedFactor; // ジャイロの回転速度にかける係数

    UserManager _userManager; // ユーザーマネージャーの参照
    float _cumulativeYaw; // Z軸回りの累積回転角度
    Vector3 _lastAcceleration;
    Vector3 _userPosition;

    public void Initialize(UserManager userManager, float userDirectionYaw, Vector3 userPosition, PDRParams pdrParams)
    {
        Input.gyro.enabled = true;

        // パラメータの初期化
        _stepLength = pdrParams.stepLength; // ステップの長さを設定
        _stepThreshold = pdrParams.stepThreshold; // ステップ検出の閾値を設定
        _rotationSpeedFactor = pdrParams.rotationSpeedFactor; // ジャイロの回転速度にかける係数を設定

        _userManager = userManager;
        _cumulativeYaw = userDirectionYaw; // 初期向きを設定
        _userPosition = userPosition; // ユーザーの初期位置を設定
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
        if (accelerationChange > _stepThreshold && !isStepping)
        {
            isStepping = true;

        }
        else if (accelerationChange < _stepThreshold * 0.5f) // 閾値を下回ったらステップ終了とみなす
        {
            isStepping = false;
        }

        _lastAcceleration = Input.acceleration;

        return isStepping;
    }

    void UpdatePosition()
    {
        Vector3 forward = new Vector3(Mathf.Cos(_cumulativeYaw), Mathf.Sin(_cumulativeYaw), 0).normalized;
        _userPosition += forward * _stepLength;
        _userManager.UpdateUserPosition(_userPosition); // ユーザーマネージャーに位置を更新
    }

    void UpdateCumulativeYaw()
    {
        _cumulativeYaw += Input.gyro.rotationRate.z * Time.deltaTime * _rotationSpeedFactor;
        Debug.Log($"Input.gyro.rotationRate.z: {Input.gyro.rotationRate.z}"); // デバッグ用ログ
        _userManager.UpdateUserDirection(_cumulativeYaw); // ユーザーマネージャーに回転を更新
    }
}
