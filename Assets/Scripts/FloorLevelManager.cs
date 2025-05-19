#nullable enable

using TMPro;
using UnityEngine;

public class FloorLevelManager : MonoBehaviour
{
    TMP_Text _floorLevelText; // フロアレベルを表示するテキスト
    GameObject _floorMaps; // フロアマップの親オブジェクト

    public void Initialize(TMP_Text floorLevelText, GameObject floorMaps)
    {
        _floorLevelText = floorLevelText;
        _floorMaps = floorMaps;
    }
    
    public void UpdateFloorLevelText(int floorLevel)
    {
        _floorLevelText.text = $"Floor Level: {floorLevel}"; // フロアレベルを表示
    }

    public void UpdateFloorLevelMap(int floorLevel)
    {
        for (int i = 0; i < _floorMaps.transform.childCount; i++)
        {
            Transform child = _floorMaps.transform.GetChild(i);
            child.gameObject.SetActive(false); // すべてのフロアマップを非表示にする 

            string floorMapName = $"FLOOR{floorLevel}";
            if(child.gameObject.name == floorMapName){
                child.gameObject.SetActive(true);
            }

        }
    }
}
