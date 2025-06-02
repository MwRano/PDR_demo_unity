# nullable enable
using UnityEngine;
using VContainer;

/// <summary>
/// PDRを行うクラス
/// </summary>
public class PDRManager
{   
    private float _stepLength; // ステップの長さ
    private float _stepThreshold; // ステップ検出の閾値（加速度の変化量）
    private float _rotationSpeedFactor; // ジャイロの回転速度にかける係数

    private UserMono _userMono;
    private Vector3 _lastAcceleration;
    private MapMatching _mapMatching;

    public bool IsMapMatchingOn { get; set; }

    [Inject]
    public PDRManager(
        PDRParams pdrParams,
        UserMono userMono,
        MapMatching mapMatching)
    {
        Input.gyro.enabled = true;

        // パラメータの初期化
        _stepLength = pdrParams.stepLength; 
        _stepThreshold = pdrParams.stepThreshold;
        _rotationSpeedFactor = pdrParams.rotationSpeedFactor;

        _userMono = userMono;
        _mapMatching = mapMatching;
        _lastAcceleration = Input.acceleration;

        IsMapMatchingOn = false;
    }

    // ステップの検知を行うメソッド
    public bool DetectStep()
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

    // 位置の更新を行うメソッド
    public void UpdatePosition()
    {
        float cumulativeYaw = _userMono.UserComulativeYaw.Value;
        Vector3 userPosition = _userMono.UserPosition.Value;
        Vector3 forward = new Vector3(Mathf.Cos(cumulativeYaw), Mathf.Sin(cumulativeYaw), 0).normalized;
        userPosition += forward * _stepLength;

        if (IsMapMatchingOn) userPosition = ProcessMapMatching(userPosition);
        _userMono.UpdateUserPosition(userPosition);
    }

    // マップマッチングの処理を行うメソッド
    private Vector3 ProcessMapMatching(Vector3 userPosition)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(userPosition);
        if (hitCollider != null)
        {
            Vector3 correctionPosition = _mapMatching.MatchUserToMap(userPosition);
            userPosition = correctionPosition;
        }

        return userPosition;
    }

    // 累積のヨー角を計算するメソッド
    public void UpdateCumulativeYaw()
    {
        float cumulativeYaw = _userMono.UserComulativeYaw.Value;
        cumulativeYaw += Input.gyro.rotationRate.z * Time.deltaTime * _rotationSpeedFactor;
        _userMono.UpdateUserDirection(cumulativeYaw);
    }
}
