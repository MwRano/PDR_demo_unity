#nullable enable

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// フロアマップ（地図）を生成するクラス
/// </summary>
public class FloorMapGenerator
{
    private List<FloorMapData> _floorMapDataList;
    private string _floorMapParentName = "FloorMaps";


    public FloorMapGenerator(List<FloorMapData> floorMapData)
    {
        _floorMapDataList = floorMapData;
    }

    // フロアマップを生成するメソッド
    public GameObject GenerateFloorMap()
    {
        GameObject floorMapParent = new GameObject(_floorMapParentName);

        foreach (var floorMapData in _floorMapDataList)
        {
            string floorMapName = $"FLOOR{floorMapData.floorId}";
            GameObject floorMapChild = new GameObject(floorMapName);
            floorMapChild.transform.parent = floorMapParent.transform;
        }

        return floorMapParent;
    }
}
