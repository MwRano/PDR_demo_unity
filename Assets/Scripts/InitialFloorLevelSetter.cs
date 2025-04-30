using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InitialFloorLevelSetter : MonoBehaviour
{
    [SerializeField] TMP_Dropdown floorLevelDropdown; // プレイヤーオブジェクト
    [SerializeField] GameObject floorMaps; // フロアマップの親オブジェクト}

    private float _initialPressure;


    void Start()
    {
        floorLevelDropdown.onValueChanged.AddListener(OnFloorSelected); // ドロップダウンの選択変更イベントにメソッドを登録
    }

    public void OnFloorSelected(int floorIndex)
    {
        // ドロップダウンの選択されたフロアレベルを取得
        int selectedFloorLevel = floorIndex + 6; // 0から始まるインデックスを6から始まるフロアレベルに変換
        UpdateInitialPressure(); // 初期気圧を更新
        UpdateFloorMapDisplay(selectedFloorLevel); // フロアマップの表示を更新
    }

    private void UpdateInitialPressure()
    {
        //_initialPressure = PressureSensor.current.atmosphericPressure.ReadValue();
    }

    private void UpdateFloorMapDisplay(int floorLevel)
    {
        for (int i = 0; i < floorMaps.transform.childCount; i++)
        {
            Transform child = floorMaps.transform.GetChild(i);
            child.gameObject.SetActive(false); // すべてのフロアマップを非表示にする 

            string floorTag = $"FLOOR{floorLevel}";
            if(child.gameObject.CompareTag(floorTag)){
                child.gameObject.SetActive(true);
            }

        }
    }
}
