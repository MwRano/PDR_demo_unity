#nullable enable
using UnityEngine;
using VContainer;
using R3;

/// <summary>
/// ユーザーの状態を管理するクラス
/// </summary>
public class UserMono : MonoBehaviour
{
    public ReactiveProperty<Vector3> UserPosition { get; set; } = null!;
    public ReactiveProperty<float> UserComulativeYaw { get; set; } = null!;

    [Inject]
    public void Initialize()
    {
        UserPosition  = new ReactiveProperty<Vector3>();;
        UserComulativeYaw = new ReactiveProperty<float>();
        Debug.Log(UserPosition);
    }

    // ユーザーの位置の更新
    public void UpdateUserPosition(Vector3 position)
    {
        UserPosition.Value = position;
        transform.position = position;
        AddVertexToLineRenderer(transform.position);
    }

    // ユーザーの向きの更新
    public void UpdateUserDirection(float cumulativeYaw)
    {
        UserComulativeYaw.Value = cumulativeYaw;
        transform.rotation
            = Quaternion.Euler(0, 0, cumulativeYaw * Mathf.Rad2Deg - 90); // 回転量の計算
    }

    // 軌跡表示用に頂点の追加
    void AddVertexToLineRenderer(Vector3 position)
    {
        if (TryGetComponent<LineRenderer>(out var lineRenderer))
        {
            // 頂点を追加する
            int vertexCount = lineRenderer.positionCount;
            lineRenderer.positionCount = vertexCount + 1; // 頂点数を増やす

            Vector3 linerendererPosition = new Vector3(position.x, position.y, -1);
            lineRenderer.SetPosition(vertexCount, linerendererPosition);
        }
    }
}
