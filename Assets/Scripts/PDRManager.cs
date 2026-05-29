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
    private float _weinbergK; // Weinberg式の係数
    private float _minStrideLength;
    private float _maxStrideLength;
    private float _strideSmoothing;
    private int _calibrationTargetSteps;
    private float _calibrationDistanceMeters;

    private UserMono _userMono;
    private Vector3 _lastAcceleration;
    private MapMatching _mapMatching;
    private bool _isStepping;
    private float _currentStepMax;
    private float _currentStepMin;
    private float _currentStrideLength;
    private bool _isCalibrating;
    private int _calibrationStepCount;
    private float _calibrationA_ppSum;

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
        _weinbergK = pdrParams.weinbergK;
        _minStrideLength = pdrParams.minStrideLength;
        _maxStrideLength = pdrParams.maxStrideLength;
        _strideSmoothing = pdrParams.strideSmoothing;
        _calibrationTargetSteps = pdrParams.calibrationSteps;
        _calibrationDistanceMeters = pdrParams.calibrationDistanceMeters;

        _userMono = userMono;
        _mapMatching = mapMatching;
        _lastAcceleration = Input.acceleration;
        _currentStepMax = _lastAcceleration.magnitude;
        _currentStepMin = _lastAcceleration.magnitude;
        _currentStrideLength = _stepLength;
        _isCalibrating = _calibrationTargetSteps > 0 && _calibrationDistanceMeters > 0f;

        IsMapMatchingOn = false;
    }

    // ステップの検知を行うメソッド
    public bool DetectStep()
    {
        bool stepDetected = false;
        float accelerationMagnitude = Input.acceleration.magnitude;
        float accelerationChange = Mathf.Abs(accelerationMagnitude - _lastAcceleration.magnitude);

        _currentStepMax = Mathf.Max(_currentStepMax, accelerationMagnitude);
        _currentStepMin = Mathf.Min(_currentStepMin, accelerationMagnitude);

        if (accelerationChange > _stepThreshold && !_isStepping)
        {
            _isStepping = true;
            stepDetected = true;

            float peakToPeak = _currentStepMax - _currentStepMin;
            UpdateStrideLength(peakToPeak);

            _currentStepMax = accelerationMagnitude;
            _currentStepMin = accelerationMagnitude;
        }
        else if (accelerationChange < _stepThreshold * 0.5f) // 閾値を下回ったらステップ終了とみなす
        {
            _isStepping = false;
        }

        _lastAcceleration = Input.acceleration;

        return stepDetected;
    }

    // 位置の更新を行うメソッド
    public void UpdatePosition()
    {
        float cumulativeYaw = _userMono.UserComulativeYaw.Value;
        Vector3 userPosition = _userMono.UserPosition.Value;
        Vector3 forward = new Vector3(Mathf.Cos(cumulativeYaw), Mathf.Sin(cumulativeYaw), 0).normalized;
        userPosition += forward * _currentStrideLength;

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

    private void UpdateStrideLength(float accelPeakToPeak)
    {
        float peakToPeak = Mathf.Max(accelPeakToPeak, 0.0001f);

        if (_isCalibrating)
        {
            _calibrationStepCount++;
            _calibrationA_ppSum += peakToPeak;

            if (_calibrationStepCount >= _calibrationTargetSteps)
            {
                float averagePeakToPeak = _calibrationA_ppSum / _calibrationStepCount;
                float targetStride = _calibrationDistanceMeters / _calibrationTargetSteps;
                if (averagePeakToPeak > 0f)
                {
                    _weinbergK = targetStride / Mathf.Sqrt(averagePeakToPeak);
                }

                _isCalibrating = false;
            }
        }

        float stride = _weinbergK > 0f ? _weinbergK * Mathf.Sqrt(peakToPeak) : _stepLength;
        float minStride = _minStrideLength > 0f ? _minStrideLength : 0f;
        float maxStride = _maxStrideLength > 0f ? _maxStrideLength : float.PositiveInfinity;
        stride = Mathf.Clamp(stride, minStride, maxStride);

        if (_strideSmoothing > 0f)
        {
            _currentStrideLength = Mathf.Lerp(_currentStrideLength, stride, _strideSmoothing);
        }
        else
        {
            _currentStrideLength = stride;
        }
    }
}
