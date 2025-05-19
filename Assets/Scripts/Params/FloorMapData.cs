using System;
using UnityEngine;

/// <summary>
/// フロアマップのデータを管理するクラス
/// </summary>
[Serializable]
public class FloorMapData
{
    [SerializeField] int floorId;
    [SerializeField] Sprite floorSprite;
}
