#nullable enable

using UnityEngine;
using TMPro;
using System.Linq;
/// <summary>
/// ユーザーの状態を管理するクラス
/// </summary>
public class UserManager : MonoBehaviour
{
    TMP_Text _userPositionText; // ユーザーの位置を表示するテキスト
    TMP_Text _userRotationText; // ユーザーの向きを表示するテキスト
    RoadSegmentCluster _roadSegmentCluster;
    MapMatching _mapMatching;

    public void Initialize(
        TMP_Text positionText,
        TMP_Text rotationText,
        RoadSegmentCluster roadSegmentCluster,
        MapMatching mapMatching)
    {
        _userPositionText = positionText;
        _userRotationText = rotationText;
        _roadSegmentCluster = roadSegmentCluster;
        _mapMatching = mapMatching;
    }

    public void UpdateUserPosition(Vector3 position)
    {
        Debug.Log($"ユーザーの位置 : {position}");
        transform.position = position;
        _userPositionText.text = $"User Position: {transform.position.x:F2}, {transform.position.y:F2}"; // 位置を表示
        AddVertexToLineRenderer(transform.position);
    }

    public void UpdateUserDirection(float cumulativeYaw)
    {
        transform.rotation = Quaternion.Euler(0, 0, cumulativeYaw * Mathf.Rad2Deg - 90); // 回転量を計算
        _userRotationText.text = $"Heading: {cumulativeYaw * Mathf.Rad2Deg:F2}°"; // ラジアンを度に変換して表示
    }

    void AddVertexToLineRenderer(Vector3 position)
    {
        if (TryGetComponent<LineRenderer>(out var lineRenderer))
        {
            // 頂点を追加する
            int vertexCount = lineRenderer.positionCount;
            lineRenderer.positionCount = vertexCount + 1; // 頂点数を増やす

            Vector3 linerendeerPosition = new Vector3(position.x, position.y, -1); // Z軸を0に設定
            lineRenderer.SetPosition(vertexCount, linerendeerPosition); // 新しい頂点を追加
            Debug.Log($"Added vertex at: {linerendeerPosition}"); // デバッグログに追加した頂点を表示
        }


    }

    // void OnTriggerExit2D(Collider2D collider)
    // {
    //     Debug.Log("歩行空間ネットワークからはみ出たよ");
    //     bool isCollisionWithRoadSegment = _roadSegmentCluster.RoadSegments
    //        .Any(roadSegment => roadSegment.gameObject == collider.gameObject);

    //     if (isCollisionWithRoadSegment)
    //     {
    //         Vector3 userPosition = _mapMatching.MatchUserToMap(transform);
    //         UpdateUserPosition(userPosition);
    //     }

    // }


}
