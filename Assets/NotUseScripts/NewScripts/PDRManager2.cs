# nullable enable

using UnityEngine;
using System;

public class PDRManager2 : MonoBehaviour
{
    // パラメータ
    private float _stepLength = 0.1f;
    private float _stepThreshold = 0.2f; // ステップ検出の閾値（加速度の変化量）
    private float _rotationSpeedFactor = 1.0f; // ジャイロの回転速度にかける係数
    
    private float _cumulativeYaw; // Z軸回りの累積回転角度
    private Vector3 _lastAcceleration;
    private Vector3 _position;

    public event Action<Vector3>? OnPositionUpdated;
    public event Action<float>? OnRotationUpdated;

    void Start()
    {
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
        _position += forward * _stepLength;
        OnPositionUpdated?.Invoke(_position);
    }

    void UpdateCumulativeYaw()
    {
        _cumulativeYaw += Input.gyro.rotationRate.z * Time.deltaTime * _rotationSpeedFactor;
        OnRotationUpdated?.Invoke(_cumulativeYaw);
    }
}
