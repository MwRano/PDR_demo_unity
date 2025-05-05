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
    private float _pressureThreshold;
    private int _currentFloorLevel;
    private float _currentFloorPressure;

    public void Initialize(FloorLevelManager floorLevelManager, int floorLevel, float floorPressure, FloorEstimationParameters floorEstimationParameters)  
    {
        _floorLevelManager = floorLevelManager;
        _currentFloorLevel = floorLevel;
        _currentFloorPressure = floorPressure; 
        _pressureThreshold = floorEstimationParameters.floorLevelPressureThreshold;
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
        return 1000f; // 仮の値を返す。実際にはセンサーからの値を取得する必要がある。
        //return PressureSensor.current.atmosphericPressure.ReadValue();
    }
}
