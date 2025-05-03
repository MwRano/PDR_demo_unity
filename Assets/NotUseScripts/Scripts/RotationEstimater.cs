using System;
using UnityEngine;

public class RotationEstimater : MonoBehaviour
{
    public float rotationSpeedFactor = 1.0f; // ジャイロの回転速度にかける係数

    private float _cumulativeYaw; // 累積したヨー角度
    private Vector3 _initialPlayerDirection; // 初期向き

    void Start()
    {
        _cumulativeYaw = Mathf.Atan2(_initialPlayerDirection.y, _initialPlayerDirection.x); // 初期向きを設定
    }

    void Update()
    {
        _cumulativeYaw += Input.gyro.rotationRate.z * Time.deltaTime * rotationSpeedFactor;
    }
}
