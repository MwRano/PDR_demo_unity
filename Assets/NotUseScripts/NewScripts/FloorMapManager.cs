using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// フロアマップを管理するクラス
/// </summary>
public class FloorMapManager : MonoBehaviour
{
    Dictionary<int, Sprite> _floorMapSprites;
    Sprite _currentFloorMapSprite;
    event Action<Sprite> OnFloorMapChanged;

    void LoadFloorMap(int floorId)
    {
        if (_floorMapSprites.TryGetValue(floorId, out var sprite))
        {
            _currentFloorMapSprite = sprite;
            OnFloorMapChanged?.Invoke(_currentFloorMapSprite);
        }
        else
        {
            Debug.LogError($"Floor map with ID {floorId} not found.");
        }
    }
}
