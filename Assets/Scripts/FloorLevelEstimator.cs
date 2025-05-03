#nullable enable

using UnityEngine;
using System;
using UnityEngine.InputSystem;

/// <summary>
/// フロアを推定するクラス
/// </summary>
public class FloorLevelEstimator : MonoBehaviour
{
    FloorLevelManager _floorLevelManager; // フロアレベルマネージャーの参照
    private float _pressureThreshold = 1000f;
    private int _currentFloorLevel;
    private float _currentFloorPressure;

    public FloorLevelEstimator(FloorLevelManager floorLevelManager, int floorLevel, float floorPressure)  
    {
        _floorLevelManager = floorLevelManager;
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

        }
        
    }

    void UpdateFloorInfo(float pressureDiff, float devicePressure)
    {
        _currentFloorLevel = pressureDiff > 0 ? _currentFloorLevel + 1 : _currentFloorLevel - 1;
        _currentFloorPressure = devicePressure;
        _floorLevelManager.UpdateFloorLevelText(_currentFloorLevel); // フロアレベルマネージャーにフロアレベルを更新
        _floorLevelManager.UpdateFloorLevelMap(_currentFloorLevel); // フロアマップを更新
    }

    float ReadPressureSensorValue()
    {
        return PressureSensor.current.atmosphericPressure.ReadValue();
    }
}
