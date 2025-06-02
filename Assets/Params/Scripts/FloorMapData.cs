using System;
using UnityEngine;

/// <summary>
/// フロアマップのデータを管理するクラス
/// </summary>
[Serializable]
public class FloorMapData
{
    public FloorMapData(
        int floorIdArg,
        GameObject floorObjArg)
    {
        floorId = floorIdArg;
        floorObj = floorObjArg;   
    }

    public int floorId;
    public GameObject floorObj;
}
