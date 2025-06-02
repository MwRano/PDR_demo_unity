#nullable enable
using UnityEngine;
using System.Collections.Generic;
using VContainer;
using UnityEngine.InputSystem;
using R3;

/// <summary>
/// フロアに関する処理を管理するクラス
/// </summary>
public class FloorManager
{
    private GameObject _floorMapBaseObj = null!;
    private float _floorMapScale;
    private List<FloorMapData> _floorMapDataList = new List<FloorMapData>();
    FloorMapParams _floorMapParam = null!;

    private float _pressureThreshold;
    private float _currentFloorPressure;

    public ReactiveProperty<FloorMapData> CurrentFloorMapData { get; set; } = new ReactiveProperty<FloorMapData>();

    [Inject]
    public FloorManager(
        FloorEstimationParams floorEstimationParams,
        FloorMapParams floorMapParam)
    {
        _pressureThreshold = floorEstimationParams.floorLevelPressureThreshold;
        _floorMapScale = floorMapParam.floorMapScale;
        _floorMapParam = floorMapParam;

        //  センサーがないときの例外処理
        if (PressureSensor.current == null)
        {
            Debug.LogWarning("気圧センサを有効化しようとしましたが、デバイスの気圧センサを認識できません。");
            return;
        }
        InputSystem.EnableDevice(PressureSensor.current);
    }

    // 階層移動を監視するメソッド
    public void MonitorFloorChange()
    {
        float devicePressure = ReadPressureSensorValue();
        float pressureDiff = _currentFloorPressure - devicePressure;

        // 閾値以上の変化でフロア移動とみなす
        bool isFloorChanged = Mathf.Abs(pressureDiff) > _pressureThreshold;
        if (isFloorChanged) UpdateFloorInfo(pressureDiff, devicePressure);
    }

    // フロアマップを描画するメソッド
    public void DisplayFloorMap()
    {
        _floorMapBaseObj = new GameObject("FloorMapBaseObj"); // このオブジェクトを親としてフロアを表示
        int index = 0;
        foreach (FloorMapData floorMapData in _floorMapParam.floorMapDataList)
        {
            GameObject floorObject
                = GameObject.Instantiate(floorMapData.floorObj, _floorMapBaseObj.transform);
            _floorMapDataList.Add(new FloorMapData(floorMapData.floorId, floorObject)); //フロアマップを登録
            floorObject.transform.localScale *= _floorMapScale;
            index++;
        }
    }

    // 現在いるフロア情報を更新するメソッド
    void UpdateFloorInfo(float pressureDiff, float devicePressure)
    {
        if (devicePressure == 0) return;

        int currentFloorLevel = 0;
        if (pressureDiff > 0)
        {
            currentFloorLevel = CurrentFloorMapData.Value.floorId + 1;
        }
        else
        {
            currentFloorLevel = CurrentFloorMapData.Value.floorId - 1;
        }

        FloorMapData newFloorMapData = _floorMapDataList.Find(data => data.floorId == currentFloorLevel);
        if (newFloorMapData == null) return;

        // 現在のフロアマップを更新
        CurrentFloorMapData.Value
            = _floorMapDataList.Find(data => data.floorId == currentFloorLevel);

        _currentFloorPressure = devicePressure;
        UpdateFloorLevelMap();
    }

    //  気圧センサ値を読み取るメソッド
    float ReadPressureSensorValue()
    {
        //  sensorがないときの例外処理
        if (PressureSensor.current == null)
        {
            Debug.LogWarning("気圧センサ値を取得しようとしましたが、デバイスの気圧センサを認識できません。仮のセンサ値を返します");
            return 1000f;
        }

        return PressureSensor.current.atmosphericPressure.ReadValue();
    }

    // フロアを設定するメソッド
    public void SetFloorLevel(int selectedIndex)
    {
        // フロアに対応したマップデータを取得
        CurrentFloorMapData.Value
            = _floorMapDataList.Find(data => data.floorId == selectedIndex + 6);

        UpdateFloorLevelMap();
        _currentFloorPressure = ReadPressureSensorValue();
    }

    // フロアマップを更新するメソッド
    public void UpdateFloorLevelMap()
    {
        for (int i = 0; i < _floorMapBaseObj.transform.childCount; i++)
        {
            Transform child = _floorMapBaseObj.transform.GetChild(i);
            child.gameObject.SetActive(false); // すべてのフロアマップを非表示にする 

            // 現在のフロアのマップを表示
            string floorMapName = $"FLOOR{CurrentFloorMapData.Value.floorId}(Clone)";
            if (child.gameObject.name == floorMapName)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
