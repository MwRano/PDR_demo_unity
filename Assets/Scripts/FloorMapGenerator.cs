#nullable enable

using UnityEngine;
using System.Collections.Generic;
using System.Numerics;

/// <summary>
/// フロアマップ（地図）を生成するクラス
/// </summary>
public class FloorMapGenerator
{
    private float _floorMapScale;
    private List<FloorMapData> _floorMapDataList;
    private string _floorMapParentName = "FloorMaps";


    public FloorMapGenerator(FloorMapParams floorMapParam)
    {
        _floorMapScale = floorMapParam.floorMapScale;
        _floorMapDataList = floorMapParam.floorMapDataList;
    }

    // フロアマップを生成するメソッド
    public GameObject GenerateFloorMap()
    {
        GameObject floorMapParent = new GameObject(_floorMapParentName);
        foreach (var floorMapData in _floorMapDataList)
        {
            InitFloorMapChild(floorMapData.floorId, floorMapData.floorObject, floorMapParent);
        }

        return floorMapParent;
    }

    //  フロアマップの初期化（名前、スケール、スプライト）するメソッド
    private void InitFloorMapChild(int floorId, GameObject floorObjectPrefab, GameObject floorMapParent)
    {
        GameObject floorObject = GameObject.Instantiate(floorObjectPrefab, floorMapParent.transform);
        floorObject.name = $"FLOOR{floorId}";
        floorObject.transform.localScale *= _floorMapScale;
        //floorObject.transform.parent = floorMapParent.transform;
    }

    
}
