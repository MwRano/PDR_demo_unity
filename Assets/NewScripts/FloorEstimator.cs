#nullable enable

using UnityEngine;
using System;
using UnityEngine.InputSystem;

// 最初の設定が終わればインスタンス化

/// <summary>
/// フロアを推定するクラス
/// </summary>
public class FloorEstimator : MonoBehaviour
{ 
    private float _pressureThreshold = 1000f;
    private int _currentFloorLevel;
    private float _currentFloorPressure;

    event Action<int>? OnFloorChanged;

    public FloorEstimator(int floorLevel, float floorPressure)  
    {
        _currentFloorLevel = floorLevel;
        _currentFloorPressure = floorPressure; 
    }

    void Update()
    {
        float devicePressure = ReadPressureSensorValue();
        float pressureDiff = _currentFloorPressure - devicePressure;
        bool isFloorChanged = Mathf.Abs(pressureDiff) > _pressureThreshold;

        if(isFloorChanged) 
        {
            UpdateFloorInfo(pressureDiff, devicePressure);
            OnFloorChanged?.Invoke(_currentFloorLevel);
        }
        
    }

    void UpdateFloorInfo(float pressureDiff, float devicePressure)
    {
        _currentFloorLevel = pressureDiff > 0 ? _currentFloorLevel + 1 : _currentFloorLevel - 1;
        _currentFloorPressure = devicePressure;
    }

    float ReadPressureSensorValue()
    {
        return PressureSensor.current.atmosphericPressure.ReadValue();
    }
}
