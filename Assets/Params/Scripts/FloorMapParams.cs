using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorMapParams", menuName = "Scriptable Objects/FloorMapParams")]
public class FloorMapParams : ScriptableObject
{
    public float floorMapScale;
    public List<FloorMapData> floorMapDataList;
}
