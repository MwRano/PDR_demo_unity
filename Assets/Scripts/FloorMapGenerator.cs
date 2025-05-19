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


    public FloorMapGenerator(FloorMapParam floorMapParam)
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
            GameObject floorMapChild = new GameObject();
            InitFloorMapChild(floorMapData.floorId, floorMapData.floorSprite, floorMapChild, floorMapParent);
        }

        return floorMapParent;
    }

    //  フロアマップの初期化（名前、スケール、スプライト）するメソッド
    private void InitFloorMapChild(int floorId, Sprite floorSprite, GameObject floorMapChild, GameObject floorMapParent)
    {
        floorMapChild.name = $"FLOOR{floorId}";
        floorMapChild.transform.localScale *= _floorMapScale;

        SpriteRenderer floorMapChildSpriteRenderer = floorMapChild.AddComponent<SpriteRenderer>();
        floorMapChildSpriteRenderer.sprite = floorSprite;

        floorMapChild.transform.parent = floorMapParent.transform;
    }

    
}
