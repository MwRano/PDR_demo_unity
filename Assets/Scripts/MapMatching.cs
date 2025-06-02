#nullable enable
using UnityEngine;
using VContainer;

/// <summary>
/// マップマッチングを行うクラス
/// </summary>
public class MapMatching
{
    private FloorManager _floorManager;

    [Inject]
    public MapMatching(FloorManager floorManager)
    {
        _floorManager = floorManager;
    }

    // userを歩行可能なセグメントにマッチングさせるメソッド
    public Vector3 MatchUserToMap(Vector3 position)
    {
        GameObject roadSegment = _floorManager.CurrentFloorMapData.Value.floorObj;

        if (roadSegment.TryGetComponent<CompositeCollider2D>(out CompositeCollider2D collider))
        {
            return collider.ClosestPoint(position);
        }

        Debug.LogWarning("マップデータに歩行可能なセグメントが設定されていません");
        return new Vector3(0, 0, 0);
    }
}
