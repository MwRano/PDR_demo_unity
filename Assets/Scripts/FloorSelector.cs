using UnityEngine;
using TMPro; 
using UnityEngine.InputSystem;

public class FloorSelector : MonoBehaviour
{
    // AppControllerからこれらの変数とる
    public float selectedFloorLevelPressure{get; set;} // 選択されたフロアレベルの気圧;
    public int selectedFloorLevel{get; set;} // 選択されたフロアレベル
    public bool isFloorLevelSet{get; set;} // フロアレベルが設定されたかどうか

    TMP_Dropdown _floorLevelDropdown;
    FloorLevelManager _floorLevelManager;

    public FloorSelector(TMP_Dropdown floorLevelDropdown, FloorLevelManager floorLevelManager)
    {
        _floorLevelDropdown = floorLevelDropdown;
        _floorLevelManager = floorLevelManager;

        _floorLevelDropdown.onValueChanged.AddListener(OnFloorSelected); // ドロップダウンの選択変更イベントにメソッドを登録
    }

    void OnFloorSelected(int floorIndex)
    {
        // ドロップダウンの選択されたフロアレベルを取得
        selectedFloorLevel = floorIndex + 6; // 0から始まるインデックスを6から始まるフロアレベルに変換
        UpdateSelectedFloorLevelPressure();

        _floorLevelManager.UpdateFloorLevelText(selectedFloorLevel); // フロアレベルを更新
        _floorLevelManager.UpdateFloorLevelMap(selectedFloorLevel); // フロアマップの表示を更新
    }

    void UpdateSelectedFloorLevelPressure()
    {
        selectedFloorLevelPressure = PressureSensor.current.atmosphericPressure.ReadValue();
    }
}
