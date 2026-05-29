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

    [SerializeField] private float positionSmoothTime = 0.12f;
    private Vector3 _targetPosition;
    private Vector3 _positionVelocity;
    private bool _hasTarget;

    [Inject]
    public void Initialize()
    {
        UserPosition  = new ReactiveProperty<Vector3>();;
        UserComulativeYaw = new ReactiveProperty<float>();
        Debug.Log(UserPosition);
    }

    private void Update()
    {
        if (!_hasTarget)
        {
            return;
        }

        if (positionSmoothTime <= 0f)
        {
            transform.position = _targetPosition;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                _targetPosition,
                ref _positionVelocity,
                positionSmoothTime);
        }

        UserPosition.Value = transform.position;
        UpdateLastVertex(transform.position);

        if ((transform.position - _targetPosition).sqrMagnitude < 0.0001f)
        {
            transform.position = _targetPosition;
            UserPosition.Value = _targetPosition;
            UpdateLastVertex(_targetPosition);
            _hasTarget = false;
        }
    }

    // ユーザーの位置の更新
    public void UpdateUserPosition(Vector3 position)
    {
        _targetPosition = position;
        _hasTarget = true;
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

    void UpdateLastVertex(Vector3 position)
    {
        if (TryGetComponent<LineRenderer>(out var lineRenderer))
        {
            int lastIndex = lineRenderer.positionCount - 1;
            if (lastIndex < 0)
            {
                return;
            }

            Vector3 linerendererPosition = new Vector3(position.x, position.y, -1);
            lineRenderer.SetPosition(lastIndex, linerendererPosition);
        }
    }
}
