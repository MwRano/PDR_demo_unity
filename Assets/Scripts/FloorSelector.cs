using UnityEngine;
using TMPro; 
using UnityEngine.InputSystem;

public class FloorSelector : MonoBehaviour
{
    public float selectedFloorLevelPressure; // 選択されたフロアレベルの気圧;
    public int selectedFloorLevel;// 選択されたフロアレベル
    public bool isFloorLevelSet;// フロアレベルが設定されたかどうか

    TMP_Dropdown _floorLevelDropdown;
    FloorLevelManager _floorLevelManager;

    public void Initialize(TMP_Dropdown floorLevelDropdown, FloorLevelManager floorLevelManager)
    {
        isFloorLevelSet = false; // 初期値を設定
        _floorLevelDropdown = floorLevelDropdown;
        _floorLevelManager = floorLevelManager;

        _floorLevelDropdown.onValueChanged.AddListener(OnFloorSelected);

        //  sensorがないときの例外処理
        if (PressureSensor.current is null)
        {
            Debug.LogWarning("気圧センサを有効化しようとしましたが、デバイスの気圧センサを認識できません。");
            return;
        }

        InputSystem.EnableDevice(PressureSensor.current);
        
    }

    void OnFloorSelected(int floorIndex)
    {
        isFloorLevelSet = true; // フロアレベルが設定されたことを示すフラグを立てる
        // ドロップダウンの選択されたフロアレベルを取得
        selectedFloorLevel = floorIndex + 6; // 0から始まるインデックスを6から始まるフロアレベルに変換
        UpdateSelectedFloorLevelPressure();

        _floorLevelManager.UpdateFloorLevelText(selectedFloorLevel); // フロアレベルを更新
        _floorLevelManager.UpdateFloorLevelMap(selectedFloorLevel); // フロアマップの表示を更新
    }

    void UpdateSelectedFloorLevelPressure()
    {
        //  sensorがないときの例外処理
        if (PressureSensor.current is null)
        {
            Debug.LogWarning("気圧センサ値を取得しようとしましたが、デバイスの気圧センサを認識できません。");
            //selectedFloorLevelPressure = 1000f;
            return;
        }
        
        selectedFloorLevelPressure = PressureSensor.current.atmosphericPressure.ReadValue();
    }
}
