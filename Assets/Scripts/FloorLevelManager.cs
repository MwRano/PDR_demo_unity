#nullable enable

using TMPro;
using UnityEngine;

public class FloorLevelManager : MonoBehaviour
{
    [SerializeField] TMP_Text floorLevelText; // フロアレベルを表示するテキスト
    [SerializeField] GameObject floorMaps; // フロアマップの親オブジェクト
    
    public void UpdateFloorLevelText(int floorLevel)
    {
        floorLevelText.text = $"Floor Level: {floorLevel}"; // フロアレベルを表示
    }

    public void UpdateFloorLevelMap(int floorLevel)
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
