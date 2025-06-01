#nullable enable
using UnityEngine;
using System.Linq;

public class MapMatching : MonoBehaviour
{
    private RoadSegmentCluster _roadSegmentCluster;

    public MapMatching(RoadSegmentCluster roadSegmentCluster)
    {
        _roadSegmentCluster = roadSegmentCluster;
    }

    public Vector3 MatchUserToMap(Vector3 position)
    {
        GameObject closestRoadSegment = _roadSegmentCluster.RoadSegment;
            // .OrderBy(roadSegment => Vector3.Distance(roadSegment.transform.position, position))
            // .FirstOrDefault();

        Debug.Log($"closest road segment : {closestRoadSegment}");
        if (closestRoadSegment.TryGetComponent<CompositeCollider2D>(out CompositeCollider2D closestCollider))
        {
            return closestCollider.ClosestPoint(position);

            // if (distanceInfo.isValid)
            // {
            //     Vector3 closestPoint = distanceInfo.pointA;
            //     return closestPoint;
            // }
        }

        Debug.LogWarning("不正な処理です");
        return new Vector3(0, 0, 0);
    }
}
